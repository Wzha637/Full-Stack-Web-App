using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using A1.Dtos;
using System.Text;

namespace A1.Helper
{
    public class VcardOutputFormatter : TextOutputFormatter
    {
        public VcardOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vcard"));// tell the service container that if 
                                                                              // you see the response header "text/vcard" then use
                                                                              // my code to convert the object to vcard format
            SupportedEncodings.Add(Encoding.UTF8);//use utf-8 to encode characters to numbers
        }



        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)//context is all the information relating to the
                                                                                                                   // process of this http call
                                                                                                                   //selectedEncoding is the encoding method used to convert text to numbers
                                                                                                                   //override this method

        {
            CardOut card = (CardOut)context.Object;//Obtain the CardOut object
            StringBuilder builder = new StringBuilder();//create a new string type variable
            builder.AppendLine("BEGIN:VCARD");
            builder.AppendLine("VERSION:4.0");
            builder.Append("N;").AppendLine(card.N);
            builder.Append("FN:").AppendLine(card.Name);
            builder.Append("UID:").AppendLine(card.Uid + "");//Remember to convert the int to string
            builder.Append("ORG:").AppendLine(card.ORG);
            builder.Append("EMAIL;TYPE=work:").AppendLine(card.Email);
            builder.Append("TEL:").AppendLine(card.Tel);
            builder.Append("URL:").AppendLine(card.URL);
            builder.Append("CATEGORIES:").AppendLine(card.Categories);
            builder.Append("PHOTO;ENCODING=BASE64;TYPE=").Append(card.PhotoType).Append(":").AppendLine(card.Photo);
            builder.Append("LOGO;ENCODING=BASE64;TYPE=PNG:").AppendLine(card.logo);
            builder.AppendLine("END:VCARD");
            string outString = builder.ToString();//convert to string
            byte[] outBytes = selectedEncoding.GetBytes(outString);// encode the string to numbers
            var response = context.HttpContext.Response.Body;//retrieve the body of the response of the http message
            return response.WriteAsync(outBytes, 0, outBytes.Length);
        }
    }
}
