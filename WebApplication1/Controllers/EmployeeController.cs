using EmployeeAPP.DAL.CModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeCModel employeemodel = new EmployeeCModel();

        public ActionResult EmployeeReg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeReg(EmployeeCModel employee)
        {

            if (employee.file.ContentLength > 0)
            {
                string watermarkText = "Nikul Darji";

                employee.FilePath = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(employee.file.FileName);

                //Read the File into a Bitmap.
                using (Image bmp = Image.FromStream(employee.file.InputStream))
                {
                    using (Graphics grp = Graphics.FromImage(bmp))
                    {
                        //Set the Color of the Watermark text.
                        Brush brush = new SolidBrush(Color.Red);

                        //Set the Font and its size.
                        Font font = new System.Drawing.Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel);

                        //Determine the size of the Watermark text.
                        SizeF textSize = new SizeF();
                        textSize = grp.MeasureString(watermarkText, font);

                        //Position the text and draw it on the image.
                        Point position = new Point((bmp.Width - ((int)textSize.Width + 10)), (bmp.Height - ((int)textSize.Height + 10)));
                        grp.DrawString(watermarkText, font, brush, position);

                        string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), employee.FilePath);
                        
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            //Save the Watermarked image to the MemoryStream.
                            bmp.Save(_path, ImageFormat.Jpeg);
                        }
                    }
                }

                employeemodel.SaveEmployee(employee);
            }

            employee = new EmployeeCModel();

            return View(employee);
        }

        public ActionResult GetAllEmployee()
        {
            List<EmployeeCModel> employeeList = new List<EmployeeCModel>();

            employeeList = employeemodel.GetAllEmployeeList();

            return View(employeeList);
        }

        [HttpGet]
        public JsonResult GeneratePassword()
        {
            return Json(GeneratePassword(true, true, true, true, false, 8), JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public static string GeneratePassword(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, int lengthOfPassword)
        {
            const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
            const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
            const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMERIC_CHARACTERS = "0123456789";
            const string SPECIAL_CHARACTERS = @"!#$%&*@\";
            const string SPACE_CHARACTER = " ";
            const int PASSWORD_LENGTH_MIN = 8;
            const int PASSWORD_LENGTH_MAX = 128;

            if (lengthOfPassword < PASSWORD_LENGTH_MIN || lengthOfPassword > PASSWORD_LENGTH_MAX)
            {
                return "Password length must be between 8 and 128.";
            }

            string characterSet = "";

            if (includeLowercase)
            {
                characterSet += LOWERCASE_CHARACTERS;
            }

            if (includeUppercase)
            {
                characterSet += UPPERCASE_CHARACTERS;
            }

            if (includeNumeric)
            {
                characterSet += NUMERIC_CHARACTERS;
            }

            if (includeSpecial)
            {
                characterSet += SPECIAL_CHARACTERS;
            }

            if (includeSpaces)
            {
                characterSet += SPACE_CHARACTER;
            }

            char[] password = new char[lengthOfPassword];
            int characterSetLength = characterSet.Length;

            Random random = new System.Random();
            for (int characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
            {
                password[characterPosition] = characterSet[random.Next(characterSetLength - 1)];
            }

            return string.Join(null, password);
        }

    }
}