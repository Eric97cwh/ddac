using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmerAdmin.Models;
using FarmerAdmin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
//for linking your AWS account
using Amazon; 
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;//appsettings.json section
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace FarmerAdmin.Controllers
{
    public class ProductController : Controller
    {
        private const string bucketName = "mybuckettp038411";
        private readonly FarmerAdminContext _context;
        public ProductController(FarmerAdminContext context)
        {
            _context = context;
        }
        //function 1: connection string to the AWS Account
        private List<string> getValues()
        {
            List<string> values = new List<string>();

            //link to the appsettings.json and get back the values
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build();
            values.Add(configure["Values:Value1"]);
            values.Add(configure["Values:Value2"]);
            values.Add(configure["Values:Value3"]);
            return values;
        }
        //instead of view all data, you can filter the data in same page
        public async Task<IActionResult> Index(string ProductName, string msg = "")
        {
            ViewBag.msg = msg;

            var productlist = from m in _context.Product select m;
            if (!string.IsNullOrEmpty(ProductName))
            {
                productlist = productlist.Where(s => s.ProductName.Contains(ProductName));
            }
            return View(productlist);
        }

        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>AddProduct([Bind("ProductName,ProductPrice,ProductPicture,ProductPictureName")] Product product, List<IFormFile> imageFile)
         {
           if (ModelState.IsValid)
            {
                //able to connect to your AWS Account
                List<string> values = getValues();
                AmazonS3Client s3Client = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);

                string filename = "";

                //read imagefile one by one
                foreach (var image in imageFile)
                {
                    //learning make a small validation before sending to the cloud S3
                    if (image.Length <= 0)
                    {
                        return BadRequest("It is an empty file. Unable to upload!");
                    }
                    else if (image.Length > 1048576) //not more than 1MB
                    {
                        return BadRequest("It is over 1MB limit of size. Unable to upload!");
                    }
                    else if (image.ContentType.ToLower() != "image/png" && image.ContentType.ToLower() != "image/jpeg"
                        && image.ContentType.ToLower() != "image/gif")
                    {
                        return BadRequest("It is not a valid image! Unable to upload!");
                    }

                    try
                    {
                        //upload to S3
                        PutObjectRequest uploadRequest = new PutObjectRequest //generate the request
                        {
                            InputStream = image.OpenReadStream(),
                            BucketName = bucketName + "/flower",
                            Key = image.FileName,
                            CannedACL = S3CannedACL.PublicRead
                        };
                        //send out the request
                        await s3Client.PutObjectAsync(uploadRequest);
                        product.ProductPicture = "https://" + bucketName + ".s3.amazonaws.com/flower/" + image.FileName;
                        product.ProductPictureName = image.FileName;
                        filename = filename + " , ";

                    }
                    catch (AmazonS3Exception ex)
                    {
                        return BadRequest("Unable to upload to S3 due to technical issue. Error message: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Unable to upload to S3 due to technical issue. Error message: " + ex.Message);
                    }
                }

           _context.Add(product);
             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));
            }
           return View(product);
         }

        //funtion - edit and update
        public async Task<IActionResult> editProduct(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            var product = await _context.Product.FindAsync(ID);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editProduct(int? ID,string productfilename, [Bind("ProductID", "ProductName,ProductPrice,ProductPicture,ProductPictureName")] Product product, List<IFormFile> imageFile)
        {
            if (ID != product.ProductID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile == null)
                    {
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        productfilename = productfilename.Replace(" ", String.Empty);
                        //able to connect to your AWS Account
                        List<string> values = getValues();
                        AmazonS3Client s3Client = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);
                        try
                        {
                            //create a delete request
                            DeleteObjectRequest deleteRequest = new DeleteObjectRequest
                            {
                                BucketName = bucketName + "/images",
                                Key = productfilename
                            };

                            await s3Client.DeleteObjectAsync(deleteRequest);
                        }
                        catch (AmazonS3Exception ex)
                        {
                            return BadRequest(ex.Message);
                        }

                        //upload new picture to s3
                        string filename = "";

                        //read imagefile one by one
                        foreach (var image in imageFile)
                        {
                            //learning make a small validation before sending to the cloud S3
                            if (image.Length <= 0)
                            {
                                return BadRequest("It is an empty file. Unable to upload!");
                            }
                            else if (image.Length > 1048576) //not more than 1MB
                            {
                                return BadRequest("It is over 1MB limit of size. Unable to upload!");
                            }
                            else if (image.ContentType.ToLower() != "image/png" && image.ContentType.ToLower() != "image/jpeg"
                                && image.ContentType.ToLower() != "image/gif")
                            {
                                return BadRequest("It is not a valid image! Unable to upload!");
                            }

                            try
                            {
                                //upload to S3
                                PutObjectRequest uploadRequest = new PutObjectRequest //generate the request
                                {
                                    InputStream = image.OpenReadStream(),
                                    BucketName = bucketName + "/images",
                                    Key = image.FileName,
                                    CannedACL = S3CannedACL.PublicRead
                                };
                                //send out the request
                                await s3Client.PutObjectAsync(uploadRequest);
                                product.ProductPicture = "https://" + bucketName + ".s3.amazonaws.com/images/" + image.FileName;
                                product.ProductPictureName = image.FileName;
                                filename = filename + " , ";

                            }
                            catch (AmazonS3Exception ex)
                            {
                                return BadRequest("Unable to upload to S3 due to technical issue. Error message: " + ex.Message);
                            }
                            catch (Exception ex)
                            {
                                return BadRequest("Unable to upload to S3 due to technical issue. Error message: " + ex.Message);
                            }
                        }
                        //
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest("Unable to update the flower of " + product.ProductName + ". Error: " + ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        //delete
        public async Task<IActionResult> deleteProduct(string ProductName, int ID, string File)
        {
            string Product_Name = ProductName;
            //able to connect to your AWS Account
            List<string> values = getValues();
            AmazonS3Client s3Client = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);
            File = File.Replace(" ", String.Empty);
            if (ID == null)
            {
                return NotFound();
            }

            try
            {
                var product = await _context.Product.FindAsync(ID);
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                try
                {
                    //create a delete request
                    DeleteObjectRequest deleteRequest = new DeleteObjectRequest
                    {
                        BucketName = bucketName + "/images",
                        Key = File
                    };

                    await s3Client.DeleteObjectAsync(deleteRequest);
                }
                catch (AmazonS3Exception ex)
                {
                    return BadRequest("S3 says" + ex.Message);
                }
                return RedirectToAction("Index", "Product", new { msg = "Product of " + Product_Name + " is deleted now!" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Product", new { msg = "Product of " + Product_Name + " is unable to delete! Error: " + ex.Message });
            }
        }
    }
}
