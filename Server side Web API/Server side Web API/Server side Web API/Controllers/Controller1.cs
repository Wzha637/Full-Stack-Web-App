using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using A1.Model;
using A1.Data;
using A1.Dtos;
using System.Drawing;
using A1.Helper;
using System.Drawing.Imaging;
using Microsoft.EntityFrameworkCore;
using System.Text;


namespace A1.Controllers
{
    [Route("api")]
    [ApiController]
    public class Controller1 : Controller
    {
        private readonly IDatabaseRepo _repository;
        DataBase context = new DataBase();
        public Controller1(IDatabaseRepo repository)
        {
            _repository = repository;
        }

        [HttpGet("GetLogo")]
        public ActionResult GetLogo()
        {
            string path = Directory.GetCurrentDirectory();
            string imgDir = Path.Combine(path, "Logo");
            string respHeader = "image/png";
            string fileName = Path.Combine(imgDir, "logo" + ".png");
            return PhysicalFile(fileName, respHeader);
        }
        [HttpGet("GetVersion")]
        public ActionResult GetVersion()
        {
            string version = "V1";
            return Ok(version);
        }
        [HttpGet("GetAllStaff")]
        public ActionResult<IEnumerable<StaffOutput>> GetAllStaff()
        {
            IEnumerable<Staff> allStaff = _repository.GetAllStaff();
            IEnumerable<StaffOutput> c = allStaff.Select(e => new StaffOutput { Id = e.Id, FirstName = e.FirstName, LastName = e.LastName, Email = e.Email, Tel = e.Tel, Title = e.Title, Url = e.Url, Research = e.Research });
            return Ok(c);
        }
        [HttpGet("GetStaffPhoto/{id}")]
        public ActionResult GetStaffPhoto(int id)
        {
            string path = Directory.GetCurrentDirectory();
            string imgDir = Path.Combine(path, "StaffPhotos");
            string fileName1 = Path.Combine(imgDir, id + ".png");
            string fileName2 = Path.Combine(imgDir, id + ".jpg");
            string fileName3 = Path.Combine(imgDir, id + ".gif");
            string original = Path.Combine(imgDir, "default.png");
            string respHeader = "";
            string fileName = "";
            if (System.IO.File.Exists(fileName1))
            {
                respHeader = "image/png";
                fileName = fileName1;
            }
            else if (System.IO.File.Exists(fileName2))
            {
                respHeader = "image/jpeg";
                fileName = fileName2;
            }
            else if (System.IO.File.Exists(fileName3))
            {
                respHeader = "image/gif";
                fileName = fileName3;
            }
            else
            {
                respHeader = "image/png";
                fileName = original;
            }
            return PhysicalFile(fileName, respHeader);
        }


        [HttpGet("GetCard/{id}")]//use http Get method, also we need to pass a id to parameter
        public ActionResult GetCard(int id)
        {
            Staff staff = _repository.GetStaff(id);// retrieve info using the method defined in repository
            string path = Directory.GetCurrentDirectory();// Find the current directory
            string fileName = Path.Combine(path, "StaffPhotos/" + id + ".jpg");// find location of the image of the staff
                                                                               // The "StaffPhotos" is the folder stroing the images
                                                                               // The id is the id unique to each bear and is  the name of each image
                                                                               // The ".jpg" suffix is the type of image 
            string original = Path.Combine(path, "StaffPhotos/" + "default" + ".png");
            CardOut cardOut = new CardOut();// create the CardOut object
            string org = "Southern Hemisphere Institue of Technology";//organisation of the staff
            string photoString, photoType, logoString, logoType;// photo string is the string that holds the base 64 encoded image 
                                                                // photo type will hold the image type eg. jpeg,png, gif
            ImageFormat imageFormat, imageFormat2;// type representing the image type in the System.Drawing package
            if (System.IO.File.Exists(fileName))// if image exists
            {
                Image image = Image.FromFile(fileName);// load image to this image object
                imageFormat = image.RawFormat;
                image = ImageHelper.Resize(image, new Size(200, 200), out photoType);//resize the image 
                photoString = ImageHelper.ImageToString(image, imageFormat);// convert image to base 64 string
                Image image2 = Image.FromFile(original);// load image to this image object
                imageFormat2 = image2.RawFormat;
                image2 = ImageHelper.Resize(image2, new Size(100, 100), out logoType);//resize the image 
                logoString = ImageHelper.ImageToString(image2, imageFormat2);// convert image to base 64 string
                cardOut.Photo = photoString;
                cardOut.PhotoType = photoType;
                cardOut.logo = logoString;
                cardOut.logoType = logoType;
                cardOut.Name = staff.Title + " " + staff.FirstName + " " + staff.LastName;
                cardOut.N = staff.LastName + ";" + staff.FirstName + ";;" + staff.Title + ";";
                cardOut.ORG = org;
                cardOut.Uid = staff.Id;
                cardOut.URL = staff.Url;
                cardOut.Tel = staff.Tel;
                cardOut.Email = staff.Email;
                cardOut.Categories = Helper.ProcessText.Filter(staff.Research);// call helper class's helperFilter method
                                                                               // to remove empty spaces in catagories of hobbies
            }
            else if (!System.IO.File.Exists(fileName))
            {
                Image image2 = Image.FromFile(original);// load image to this image object
                imageFormat2 = image2.RawFormat;
                image2 = ImageHelper.Resize(image2, new Size(100, 100), out logoType);//resize the image 
                logoString = ImageHelper.ImageToString(image2, imageFormat2);// convert image to base 64 string
                cardOut.PhotoType = "JPEG";
                cardOut.logo = logoString;
                cardOut.logoType = logoType;
                cardOut.N = ";;;;";
            }
            Response.Headers.Add("Content-Type", "text/vcard");//add header called Content type 
            return Ok(cardOut);// return CardOut object
        }
        [HttpGet("GetItems/{name}")]
        public ActionResult<IEnumerable<Products>> GetItems(string name)
        {
            IEnumerable<Products> allProducts = _repository.GetAllItems();
            if (String.IsNullOrWhiteSpace(name) == true) {
                IEnumerable<ProductsOutput> c = allProducts.Select(e => new ProductsOutput { Id = e.Id, Name = e.Name, Description = e.Description, Price = e.Price });
                return Ok(c);
            }
            else
            {
                return context.AllProducts.Where(e => EF.Functions.Like(e.Name, "%" + name + "%")).ToArray();

            }
        }
        [HttpGet("GetItems")]
        public ActionResult<IEnumerable<ProductsOutput>> GetAllItems(string name)
        {
            IEnumerable<Products> allProducts = _repository.GetAllItems();
            IEnumerable<ProductsOutput> c = allProducts.Select(e => new ProductsOutput { Id = e.Id, Name = e.Name, Description = e.Description, Price = e.Price });
            return Ok(c);
        }
        [HttpGet("GetItemPhoto/{id}")]
        public ActionResult GetItemPhoto(int id)
        {
            string path = Directory.GetCurrentDirectory();
            string imgDir = Path.Combine(path, "ItemsImages");
            string fileName1 = Path.Combine(imgDir, id + ".png");
            string fileName2 = Path.Combine(imgDir, id + ".jpg");
            string fileName3 = Path.Combine(imgDir, id + ".gif");
            string original = Path.Combine(imgDir, "default.png");
            string respHeader = "";
            string fileName = "";
            if (System.IO.File.Exists(fileName1))
            {
                respHeader = "image/png";
                fileName = fileName1;
            }
            else if (System.IO.File.Exists(fileName2))
            {
                respHeader = "image/jpeg";
                fileName = fileName2;
            }
            else if (System.IO.File.Exists(fileName3))
            {
                respHeader = "image/gif";
                fileName = fileName3;
            }
            else
            {
                respHeader = "image/png";
                fileName = original;
            }
            return PhysicalFile(fileName, respHeader);
        }
        [HttpPost("WriteComment")]
        public ActionResult WriteComment(CommentsInput comment)
        {
            Comments newComment = new Comments { Comment = comment.Comment, Name = comment.Name, IP = HttpContext.Connection.RemoteIpAddress.ToString(), Time = DateTime.Now };
            context.Comments.Add(newComment);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetComments), new { id = newComment.Id }, newComment);
        }

        [HttpGet("GetComments")]
        public ActionResult GetComments()
        {
            List<Comments> comments = context.Comments.OrderByDescending(e => e.Time).Take(5).ToList();
            string content = @"<html>
                                     <head><title></title></head>";
            foreach(Comments c in comments)
            {
                content += @"<p>" + c.Comment + @" &mdash; " + c.Name + @"</p>";
                
            }
            content += @"</body></html>";
            return Content(content, "text/html", Encoding.UTF8);
        }
    }
}
