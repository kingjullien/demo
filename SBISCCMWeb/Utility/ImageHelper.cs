using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
namespace SBISCCMWeb.Utility
{
    public class ImageHelper
    {
        public static string ResizeImageThumb(string srcPath)
        {

            const int thumbWidth = 200;
            const string suffix = "thumb";

            var extension = Path.GetExtension(srcPath);
            if (extension != null)
            {
                string savePath = srcPath.Replace(extension, "-" + suffix + extension.ToLower());

                using (Image image = Image.FromFile(srcPath))
                {
                    int srcWidth = image.Width;
                    int srcHeight = image.Height;
                    int thumbHeight = (int)((srcHeight / (double)srcWidth) * thumbWidth);

                    using (Bitmap bmp = new Bitmap(thumbWidth, thumbHeight))
                    {
                        bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                        using (var gr = Graphics.FromImage(bmp))
                        {
                            //set the resize quality modes to high quality
                            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                            gr.DrawImage(image, 0, 0, thumbWidth, thumbHeight);
                        }

                        if (ImageFormat.Jpeg.Equals(image.RawFormat))
                        {
                            // SaveJpeg(destPath, bmp, 95);

                            //create an encoder parameter for the image quality
                            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, 95L);
                            //get the jpeg codec
                            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

                            //create a collection of all parameters that we will pass to the encoder
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            //set the quality parameter for the codec
                            encoderParams.Param[0] = qualityParam;
                            //save the image using the codec and the parameters
                            bmp.Save(savePath, jpegCodec, encoderParams);
                        }
                        else
                            bmp.Save(savePath, image.RawFormat);
                    }
                }
                return savePath;
            }
            return null;
        }

        /// <summary>
        /// Remove actual image and thumbnail image
        /// </summary>
        /// <param name="fullPath"></param>
        public static void RemoveImages(string fullPath)
        {
            if (!string.IsNullOrEmpty(fullPath))
            {
                //Remove Main Image
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                //Remove Thumb Image
                if (File.Exists(GetThumbName(fullPath)))
                {
                    File.Delete(GetThumbName(fullPath));
                }
            }
        }

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        private static Dictionary<string, ImageCodecInfo> _encoders;

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        private static Dictionary<string, ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demand
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (_encoders == null)
                {
                    _encoders = new Dictionary<string, ImageCodecInfo>();
                }

                //if there are no codecs, try loading them
                if (_encoders.Count == 0)
                {
                    //get all the codecs
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                    {
                        //add each codec to the quick lookup
                        _encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }

                //return the lookup
                return _encoders;
            }
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();

            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }

        public enum PictureType
        {

            clientlogo,
            TicketImage
        }

        private static Stream ResizeImage(Stream imageStream, int thumbWidth)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(imageStream))
            {
                int srcWidth = image.Width;

                if (image.Width <= thumbWidth)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    image.Save(memoryStream, image.RawFormat);
                    return memoryStream;
                    //return imageStream;
                }

                int srcHeight = image.Height;
                int thumbHeight = (int)(((double)srcHeight / (double)srcWidth) * (double)thumbWidth);

                using (Bitmap bmp = new Bitmap(thumbWidth, thumbHeight))
                {
                    bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                    using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp))
                    {
                        //set the resize quality modes to high quality
                        gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        gr.DrawImage(image, 0, 0, thumbWidth, thumbHeight);
                    }

                    MemoryStream memoryStream = new MemoryStream();

                    if (ImageFormat.Jpeg.Equals(image.RawFormat))
                    {
                        // SaveJpeg(destPath, bmp, 95);

                        //create an encoder parameter for the image quality
                        EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);
                        //get the jpeg codec
                        ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

                        //create a collection of all parameters that we will pass to the encoder
                        EncoderParameters encoderParams = new EncoderParameters(1);
                        //set the quality parameter for the codec
                        encoderParams.Param[0] = qualityParam;
                        //save the image using the codec and the parameters
                        bmp.Save(memoryStream, jpegCodec, encoderParams);
                    }
                    else
                        bmp.Save(memoryStream, image.RawFormat);

                    return memoryStream;
                }
            }

        }

        public static string UploadNONImage(HttpPostedFileBase file, string fileNameToSave, PictureType pictureType)
        {
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                string strExtension = Path.GetExtension(fileNameToSave).ToLower();
                string dirPath = string.Empty;

                dirPath = pictureType.ToString().ToLowerInvariant();

                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(dirPath);
                if (!container.Exists())
                    container.Create(BlobContainerPublicAccessType.Blob);

                file.InputStream.Seek(0, SeekOrigin.Begin);
                CloudBlockBlob thumbBlob = container.GetBlockBlobReference(fileNameToSave);
                thumbBlob.Properties.CacheControl = "public, max-age=31536000";
                thumbBlob.Properties.ContentType = file.ContentType;
                thumbBlob.UploadFromStream(file.InputStream);

                return fileNameToSave;
            }
            else
                return string.Empty;
        }

        public static string UploadImage(HttpPostedFileBase file, string fileNameToSave, PictureType pictureType)
        {
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                string strExtension = Path.GetExtension(fileNameToSave).ToLower();
                string dirPath = string.Empty;

                dirPath = pictureType.ToString().ToLowerInvariant();

                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(dirPath);
                if (!container.Exists())
                    container.Create(BlobContainerPublicAccessType.Blob);

                string filename = fileNameToSave.Replace(strExtension, "-thumb" + strExtension);


                Stream resizedImage = ResizeImage(file.InputStream, 200);
                resizedImage.Seek(0, SeekOrigin.Begin);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
                blockBlob.Properties.CacheControl = "public, max-age=31536000";
                blockBlob.Properties.ContentType = file.ContentType;
                blockBlob.UploadFromStream(resizedImage);


                file.InputStream.Seek(0, SeekOrigin.Begin);
                CloudBlockBlob thumbBlob = container.GetBlockBlobReference(fileNameToSave);
                thumbBlob.Properties.CacheControl = "public, max-age=31536000";
                thumbBlob.Properties.ContentType = file.ContentType;
                thumbBlob.UploadFromStream(file.InputStream);

                return fileNameToSave;
            }
            else
                return string.Empty;
        }

        public static string UploadImageStream(Stream inputStream, string fileNameToSave, PictureType pictureType)
        {
            if (inputStream != null && inputStream.Length > 0)
            {
                string strExtension = Path.GetExtension(fileNameToSave).ToLower();
                string mimeType = GetMimeType(strExtension);
                string dirPath = string.Empty;

                dirPath = pictureType.ToString().ToLowerInvariant();

                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(dirPath);
                if (!container.Exists())
                    container.Create(BlobContainerPublicAccessType.Blob);

                string filename = fileNameToSave.Replace(strExtension, "-thumb" + strExtension);


                Stream resizedImage = ResizeImage(inputStream, 200);
                resizedImage.Seek(0, SeekOrigin.Begin);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

                blockBlob.Properties.CacheControl = "public, max-age=31536000";
                if (!string.IsNullOrEmpty(mimeType))
                    blockBlob.Properties.ContentType = mimeType;
                blockBlob.UploadFromStream(resizedImage);


                inputStream.Seek(0, SeekOrigin.Begin);
                CloudBlockBlob thumbBlob = container.GetBlockBlobReference(fileNameToSave);
                thumbBlob.Properties.CacheControl = "public, max-age=31536000";
                if (!string.IsNullOrEmpty(mimeType))
                    thumbBlob.Properties.ContentType = mimeType;
                thumbBlob.UploadFromStream(inputStream);

                return fileNameToSave;
            }
            else
                return string.Empty;
        }

        public static string UploadImageStreamWITHOUTThumb(Stream inputStream, string fileNameToSave, PictureType pictureType)
        {
            if (inputStream != null && inputStream.Length > 0)
            {
                string strExtension = Path.GetExtension(fileNameToSave).ToLower();
                string mimeType = GetMimeType(strExtension);
                string dirPath = string.Empty;

                dirPath = pictureType.ToString().ToLowerInvariant();

                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(dirPath);
                if (!container.Exists())
                    container.Create(BlobContainerPublicAccessType.Blob);

                inputStream.Seek(0, SeekOrigin.Begin);
                CloudBlockBlob thumbBlob = container.GetBlockBlobReference(fileNameToSave);
                thumbBlob.Properties.CacheControl = "public, max-age=31536000";
                if (!string.IsNullOrEmpty(mimeType))
                    thumbBlob.Properties.ContentType = mimeType;
                thumbBlob.UploadFromStream(inputStream);

                return fileNameToSave;
            }
            else
                return string.Empty;
        }

        public static string GetImageThumbURL(PictureType pictureType, string imageName)
        {
            if (!string.IsNullOrWhiteSpace(imageName))
                return GetWebConfigKeyValue("AzureStoragePath") + pictureType.ToString().ToLowerInvariant() + "/" + GetThumbName(imageName);
            else
                return string.Empty;
        }

        public static string GetFileURL(PictureType pictureType, string imageName)
        {
            if (!string.IsNullOrWhiteSpace(imageName))
                return GetWebConfigKeyValue("AzureStoragePath") + pictureType.ToString().ToLowerInvariant() + "/" + imageName;
            else
                return string.Empty;
        }

        public static string GetImageFullURL(PictureType pictureType, string imageName)
        {
            if (!string.IsNullOrWhiteSpace(imageName))
                return GetWebConfigKeyValue("AzureStoragePath") + pictureType.ToString().ToLowerInvariant() + "/" + imageName;
            else
                return string.Empty;
        }

        public static void CreateContainers()
        {

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            foreach (var value in Enum.GetValues(typeof(PictureType)))
            {
                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(value.ToString().ToLowerInvariant());
                if (!container.Exists())
                    container.Create(BlobContainerPublicAccessType.Blob);

                container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            }
        }

        private static IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {

        #region Big freaking list of mime types
        // combination of values from Windows 7 Registry and 
        // from C:\Windows\System32\inetsrv\config\applicationHost.config
        // some added, including .7z and .dat
        {".323", "text/h323"},
        {".3g2", "video/3gpp2"},
        {".3gp", "video/3gpp"},
        {".3gp2", "video/3gpp2"},
        {".3gpp", "video/3gpp"},
        {".7z", "application/x-7z-compressed"},
        {".aa", "audio/audible"},
        {".AAC", "audio/aac"},
        {".aaf", "application/octet-stream"},
        {".aax", "audio/vnd.audible.aax"},
        {".ac3", "audio/ac3"},
        {".aca", "application/octet-stream"},
        {".accda", "application/msaccess.addin"},
        {".accdb", "application/msaccess"},
        {".accdc", "application/msaccess.cab"},
        {".accde", "application/msaccess"},
        {".accdr", "application/msaccess.runtime"},
        {".accdt", "application/msaccess"},
        {".accdw", "application/msaccess.webapplication"},
        {".accft", "application/msaccess.ftemplate"},
        {".acx", "application/internet-property-stream"},
        {".AddIn", "text/xml"},
        {".ade", "application/msaccess"},
        {".adobebridge", "application/x-bridge-url"},
        {".adp", "application/msaccess"},
        {".ADT", "audio/vnd.dlna.adts"},
        {".ADTS", "audio/aac"},
        {".afm", "application/octet-stream"},
        {".ai", "application/postscript"},
        {".aif", "audio/x-aiff"},
        {".aifc", "audio/aiff"},
        {".aiff", "audio/aiff"},
        {".air", "application/vnd.adobe.air-application-installer-package+zip"},
        {".amc", "application/x-mpeg"},
        {".application", "application/x-ms-application"},
        {".art", "image/x-jg"},
        {".asa", "application/xml"},
        {".asax", "application/xml"},
        {".ascx", "application/xml"},
        {".asd", "application/octet-stream"},
        {".asf", "video/x-ms-asf"},
        {".ashx", "application/xml"},
        {".asi", "application/octet-stream"},
        {".asm", "text/plain"},
        {".asmx", "application/xml"},
        {".aspx", "application/xml"},
        {".asr", "video/x-ms-asf"},
        {".asx", "video/x-ms-asf"},
        {".atom", "application/atom+xml"},
        {".au", "audio/basic"},
        {".avi", "video/x-msvideo"},
        {".axs", "application/olescript"},
        {".bas", "text/plain"},
        {".bcpio", "application/x-bcpio"},
        {".bin", "application/octet-stream"},
        {".bmp", "image/bmp"},
        {".c", "text/plain"},
        {".cab", "application/octet-stream"},
        {".caf", "audio/x-caf"},
        {".calx", "application/vnd.ms-office.calx"},
        {".cat", "application/vnd.ms-pki.seccat"},
        {".cc", "text/plain"},
        {".cd", "text/plain"},
        {".cdda", "audio/aiff"},
        {".cdf", "application/x-cdf"},
        {".cer", "application/x-x509-ca-cert"},
        {".chm", "application/octet-stream"},
        {".class", "application/x-java-applet"},
        {".clp", "application/x-msclip"},
        {".cmx", "image/x-cmx"},
        {".cnf", "text/plain"},
        {".cod", "image/cis-cod"},
        {".config", "application/xml"},
        {".contact", "text/x-ms-contact"},
        {".coverage", "application/xml"},
        {".cpio", "application/x-cpio"},
        {".cpp", "text/plain"},
        {".crd", "application/x-mscardfile"},
        {".crl", "application/pkix-crl"},
        {".crt", "application/x-x509-ca-cert"},
        {".cs", "text/plain"},
        {".csdproj", "text/plain"},
        {".csh", "application/x-csh"},
        {".csproj", "text/plain"},
        {".css", "text/css"},
        {".csv", "text/csv"},
        {".cur", "application/octet-stream"},
        {".cxx", "text/plain"},
        {".dat", "application/octet-stream"},
        {".datasource", "application/xml"},
        {".dbproj", "text/plain"},
        {".dcr", "application/x-director"},
        {".def", "text/plain"},
        {".deploy", "application/octet-stream"},
        {".der", "application/x-x509-ca-cert"},
        {".dgml", "application/xml"},
        {".dib", "image/bmp"},
        {".dif", "video/x-dv"},
        {".dir", "application/x-director"},
        {".disco", "text/xml"},
        {".dll", "application/x-msdownload"},
        {".dll.config", "text/xml"},
        {".dlm", "text/dlm"},
        {".doc", "application/msword"},
        {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
        {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
        {".dot", "application/msword"},
        {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
        {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
        {".dsp", "application/octet-stream"},
        {".dsw", "text/plain"},
        {".dtd", "text/xml"},
        {".dtsConfig", "text/xml"},
        {".dv", "video/x-dv"},
        {".dvi", "application/x-dvi"},
        {".dwf", "drawing/x-dwf"},
        {".dwp", "application/octet-stream"},
        {".dxr", "application/x-director"},
        {".eml", "message/rfc822"},
        {".emz", "application/octet-stream"},
        {".eot", "application/octet-stream"},
        {".eps", "application/postscript"},
        {".etl", "application/etl"},
        {".etx", "text/x-setext"},
        {".evy", "application/envoy"},
        {".exe", "application/octet-stream"},
        {".exe.config", "text/xml"},
        {".fdf", "application/vnd.fdf"},
        {".fif", "application/fractals"},
        {".filters", "Application/xml"},
        {".fla", "application/octet-stream"},
        {".flr", "x-world/x-vrml"},
        {".flv", "video/x-flv"},
        {".fsscript", "application/fsharp-script"},
        {".fsx", "application/fsharp-script"},
        {".generictest", "application/xml"},
        {".gif", "image/gif"},
        {".group", "text/x-ms-group"},
        {".gsm", "audio/x-gsm"},
        {".gtar", "application/x-gtar"},
        {".gz", "application/x-gzip"},
        {".h", "text/plain"},
        {".hdf", "application/x-hdf"},
        {".hdml", "text/x-hdml"},
        {".hhc", "application/x-oleobject"},
        {".hhk", "application/octet-stream"},
        {".hhp", "application/octet-stream"},
        {".hlp", "application/winhlp"},
        {".hpp", "text/plain"},
        {".hqx", "application/mac-binhex40"},
        {".hta", "application/hta"},
        {".htc", "text/x-component"},
        {".htm", "text/html"},
        {".html", "text/html"},
        {".htt", "text/webviewhtml"},
        {".hxa", "application/xml"},
        {".hxc", "application/xml"},
        {".hxd", "application/octet-stream"},
        {".hxe", "application/xml"},
        {".hxf", "application/xml"},
        {".hxh", "application/octet-stream"},
        {".hxi", "application/octet-stream"},
        {".hxk", "application/xml"},
        {".hxq", "application/octet-stream"},
        {".hxr", "application/octet-stream"},
        {".hxs", "application/octet-stream"},
        {".hxt", "text/html"},
        {".hxv", "application/xml"},
        {".hxw", "application/octet-stream"},
        {".hxx", "text/plain"},
        {".i", "text/plain"},
        {".ico", "image/x-icon"},
        {".ics", "application/octet-stream"},
        {".idl", "text/plain"},
        {".ief", "image/ief"},
        {".iii", "application/x-iphone"},
        {".inc", "text/plain"},
        {".inf", "application/octet-stream"},
        {".inl", "text/plain"},
        {".ins", "application/x-internet-signup"},
        {".ipa", "application/x-itunes-ipa"},
        {".ipg", "application/x-itunes-ipg"},
        {".ipproj", "text/plain"},
        {".ipsw", "application/x-itunes-ipsw"},
        {".iqy", "text/x-ms-iqy"},
        {".isp", "application/x-internet-signup"},
        {".ite", "application/x-itunes-ite"},
        {".itlp", "application/x-itunes-itlp"},
        {".itms", "application/x-itunes-itms"},
        {".itpc", "application/x-itunes-itpc"},
        {".IVF", "video/x-ivf"},
        {".jar", "application/java-archive"},
        {".java", "application/octet-stream"},
        {".jck", "application/liquidmotion"},
        {".jcz", "application/liquidmotion"},
        {".jfif", "image/pjpeg"},
        {".jnlp", "application/x-java-jnlp-file"},
        {".jpb", "application/octet-stream"},
        {".jpe", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpeg"},
        {".js", "application/x-javascript"},
        {".json", "application/json"},
        {".jsx", "text/jscript"},
        {".jsxbin", "text/plain"},
        {".latex", "application/x-latex"},
        {".library-ms", "application/windows-library+xml"},
        {".lit", "application/x-ms-reader"},
        {".loadtest", "application/xml"},
        {".lpk", "application/octet-stream"},
        {".lsf", "video/x-la-asf"},
        {".lst", "text/plain"},
        {".lsx", "video/x-la-asf"},
        {".lzh", "application/octet-stream"},
        {".m13", "application/x-msmediaview"},
        {".m14", "application/x-msmediaview"},
        {".m1v", "video/mpeg"},
        {".m2t", "video/vnd.dlna.mpeg-tts"},
        {".m2ts", "video/vnd.dlna.mpeg-tts"},
        {".m2v", "video/mpeg"},
        {".m3u", "audio/x-mpegurl"},
        {".m3u8", "audio/x-mpegurl"},
        {".m4a", "audio/m4a"},
        {".m4b", "audio/m4b"},
        {".m4p", "audio/m4p"},
        {".m4r", "audio/x-m4r"},
        {".m4v", "video/x-m4v"},
        {".mac", "image/x-macpaint"},
        {".mak", "text/plain"},
        {".man", "application/x-troff-man"},
        {".manifest", "application/x-ms-manifest"},
        {".map", "text/plain"},
        {".master", "application/xml"},
        {".mda", "application/msaccess"},
        {".mdb", "application/x-msaccess"},
        {".mde", "application/msaccess"},
        {".mdp", "application/octet-stream"},
        {".me", "application/x-troff-me"},
        {".mfp", "application/x-shockwave-flash"},
        {".mht", "message/rfc822"},
        {".mhtml", "message/rfc822"},
        {".mid", "audio/mid"},
        {".midi", "audio/mid"},
        {".mix", "application/octet-stream"},
        {".mk", "text/plain"},
        {".mmf", "application/x-smaf"},
        {".mno", "text/xml"},
        {".mny", "application/x-msmoney"},
        {".mod", "video/mpeg"},
        {".mov", "video/quicktime"},
        {".movie", "video/x-sgi-movie"},
        {".mp2", "video/mpeg"},
        {".mp2v", "video/mpeg"},
        {".mp3", "audio/mpeg"},
        {".mp4", "video/mp4"},
        {".mp4v", "video/mp4"},
        {".mpa", "video/mpeg"},
        {".mpe", "video/mpeg"},
        {".mpeg", "video/mpeg"},
        {".mpf", "application/vnd.ms-mediapackage"},
        {".mpg", "video/mpeg"},
        {".mpp", "application/vnd.ms-project"},
        {".mpv2", "video/mpeg"},
        {".mqv", "video/quicktime"},
        {".ms", "application/x-troff-ms"},
        {".msi", "application/octet-stream"},
        {".mso", "application/octet-stream"},
        {".mts", "video/vnd.dlna.mpeg-tts"},
        {".mtx", "application/xml"},
        {".mvb", "application/x-msmediaview"},
        {".mvc", "application/x-miva-compiled"},
        {".mxp", "application/x-mmxp"},
        {".nc", "application/x-netcdf"},
        {".nsc", "video/x-ms-asf"},
        {".nws", "message/rfc822"},
        {".ocx", "application/octet-stream"},
        {".oda", "application/oda"},
        {".odc", "text/x-ms-odc"},
        {".odh", "text/plain"},
        {".odl", "text/plain"},
        {".odp", "application/vnd.oasis.opendocument.presentation"},
        {".ods", "application/oleobject"},
        {".odt", "application/vnd.oasis.opendocument.text"},
        {".one", "application/onenote"},
        {".onea", "application/onenote"},
        {".onepkg", "application/onenote"},
        {".onetmp", "application/onenote"},
        {".onetoc", "application/onenote"},
        {".onetoc2", "application/onenote"},
        {".orderedtest", "application/xml"},
        {".osdx", "application/opensearchdescription+xml"},
        {".p10", "application/pkcs10"},
        {".p12", "application/x-pkcs12"},
        {".p7b", "application/x-pkcs7-certificates"},
        {".p7c", "application/pkcs7-mime"},
        {".p7m", "application/pkcs7-mime"},
        {".p7r", "application/x-pkcs7-certreqresp"},
        {".p7s", "application/pkcs7-signature"},
        {".pbm", "image/x-portable-bitmap"},
        {".pcast", "application/x-podcast"},
        {".pct", "image/pict"},
        {".pcx", "application/octet-stream"},
        {".pcz", "application/octet-stream"},
        {".pdf", "application/pdf"},
        {".pfb", "application/octet-stream"},
        {".pfm", "application/octet-stream"},
        {".pfx", "application/x-pkcs12"},
        {".pgm", "image/x-portable-graymap"},
        {".pic", "image/pict"},
        {".pict", "image/pict"},
        {".pkgdef", "text/plain"},
        {".pkgundef", "text/plain"},
        {".pko", "application/vnd.ms-pki.pko"},
        {".pls", "audio/scpls"},
        {".pma", "application/x-perfmon"},
        {".pmc", "application/x-perfmon"},
        {".pml", "application/x-perfmon"},
        {".pmr", "application/x-perfmon"},
        {".pmw", "application/x-perfmon"},
        {".png", "image/png"},
        {".pnm", "image/x-portable-anymap"},
        {".pnt", "image/x-macpaint"},
        {".pntg", "image/x-macpaint"},
        {".pnz", "image/png"},
        {".pot", "application/vnd.ms-powerpoint"},
        {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
        {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
        {".ppa", "application/vnd.ms-powerpoint"},
        {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
        {".ppm", "image/x-portable-pixmap"},
        {".pps", "application/vnd.ms-powerpoint"},
        {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
        {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
        {".ppt", "application/vnd.ms-powerpoint"},
        {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
        {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
        {".prf", "application/pics-rules"},
        {".prm", "application/octet-stream"},
        {".prx", "application/octet-stream"},
        {".ps", "application/postscript"},
        {".psc1", "application/PowerShell"},
        {".psd", "application/octet-stream"},
        {".psess", "application/xml"},
        {".psm", "application/octet-stream"},
        {".psp", "application/octet-stream"},
        {".pub", "application/x-mspublisher"},
        {".pwz", "application/vnd.ms-powerpoint"},
        {".qht", "text/x-html-insertion"},
        {".qhtm", "text/x-html-insertion"},
        {".qt", "video/quicktime"},
        {".qti", "image/x-quicktime"},
        {".qtif", "image/x-quicktime"},
        {".qtl", "application/x-quicktimeplayer"},
        {".qxd", "application/octet-stream"},
        {".ra", "audio/x-pn-realaudio"},
        {".ram", "audio/x-pn-realaudio"},
        {".rar", "application/octet-stream"},
        {".ras", "image/x-cmu-raster"},
        {".rat", "application/rat-file"},
        {".rc", "text/plain"},
        {".rc2", "text/plain"},
        {".rct", "text/plain"},
        {".rdlc", "application/xml"},
        {".resx", "application/xml"},
        {".rf", "image/vnd.rn-realflash"},
        {".rgb", "image/x-rgb"},
        {".rgs", "text/plain"},
        {".rm", "application/vnd.rn-realmedia"},
        {".rmi", "audio/mid"},
        {".rmp", "application/vnd.rn-rn_music_package"},
        {".roff", "application/x-troff"},
        {".rpm", "audio/x-pn-realaudio-plugin"},
        {".rqy", "text/x-ms-rqy"},
        {".rtf", "application/rtf"},
        {".rtx", "text/richtext"},
        {".ruleset", "application/xml"},
        {".s", "text/plain"},
        {".safariextz", "application/x-safari-safariextz"},
        {".scd", "application/x-msschedule"},
        {".sct", "text/scriptlet"},
        {".sd2", "audio/x-sd2"},
        {".sdp", "application/sdp"},
        {".sea", "application/octet-stream"},
        {".searchConnector-ms", "application/windows-search-connector+xml"},
        {".setpay", "application/set-payment-initiation"},
        {".setreg", "application/set-registration-initiation"},
        {".settings", "application/xml"},
        {".sgimb", "application/x-sgimb"},
        {".sgml", "text/sgml"},
        {".sh", "application/x-sh"},
        {".shar", "application/x-shar"},
        {".shtml", "text/html"},
        {".sit", "application/x-stuffit"},
        {".sitemap", "application/xml"},
        {".skin", "application/xml"},
        {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
        {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
        {".slk", "application/vnd.ms-excel"},
        {".sln", "text/plain"},
        {".slupkg-ms", "application/x-ms-license"},
        {".smd", "audio/x-smd"},
        {".smi", "application/octet-stream"},
        {".smx", "audio/x-smd"},
        {".smz", "audio/x-smd"},
        {".snd", "audio/basic"},
        {".snippet", "application/xml"},
        {".snp", "application/octet-stream"},
        {".sol", "text/plain"},
        {".sor", "text/plain"},
        {".spc", "application/x-pkcs7-certificates"},
        {".spl", "application/futuresplash"},
        {".src", "application/x-wais-source"},
        {".srf", "text/plain"},
        {".SSISDeploymentManifest", "text/xml"},
        {".ssm", "application/streamingmedia"},
        {".sst", "application/vnd.ms-pki.certstore"},
        {".stl", "application/vnd.ms-pki.stl"},
        {".sv4cpio", "application/x-sv4cpio"},
        {".sv4crc", "application/x-sv4crc"},
        {".svc", "application/xml"},
        {".swf", "application/x-shockwave-flash"},
        {".t", "application/x-troff"},
        {".tar", "application/x-tar"},
        {".tcl", "application/x-tcl"},
        {".testrunconfig", "application/xml"},
        {".testsettings", "application/xml"},
        {".tex", "application/x-tex"},
        {".texi", "application/x-texinfo"},
        {".texinfo", "application/x-texinfo"},
        {".tgz", "application/x-compressed"},
        {".thmx", "application/vnd.ms-officetheme"},
        {".thn", "application/octet-stream"},
        {".tif", "image/tiff"},
        {".tiff", "image/tiff"},
        {".tlh", "text/plain"},
        {".tli", "text/plain"},
        {".toc", "application/octet-stream"},
        {".tr", "application/x-troff"},
        {".trm", "application/x-msterminal"},
        {".trx", "application/xml"},
        {".ts", "video/vnd.dlna.mpeg-tts"},
        {".tsv", "text/tab-separated-values"},
        {".ttf", "application/octet-stream"},
        {".tts", "video/vnd.dlna.mpeg-tts"},
        {".txt", "text/plain"},
        {".u32", "application/octet-stream"},
        {".uls", "text/iuls"},
        {".user", "text/plain"},
        {".ustar", "application/x-ustar"},
        {".vb", "text/plain"},
        {".vbdproj", "text/plain"},
        {".vbk", "video/mpeg"},
        {".vbproj", "text/plain"},
        {".vbs", "text/vbscript"},
        {".vcf", "text/x-vcard"},
        {".vcproj", "Application/xml"},
        {".vcs", "text/plain"},
        {".vcxproj", "Application/xml"},
        {".vddproj", "text/plain"},
        {".vdp", "text/plain"},
        {".vdproj", "text/plain"},
        {".vdx", "application/vnd.ms-visio.viewer"},
        {".vml", "text/xml"},
        {".vscontent", "application/xml"},
        {".vsct", "text/xml"},
        {".vsd", "application/vnd.visio"},
        {".vsi", "application/ms-vsi"},
        {".vsix", "application/vsix"},
        {".vsixlangpack", "text/xml"},
        {".vsixmanifest", "text/xml"},
        {".vsmdi", "application/xml"},
        {".vspscc", "text/plain"},
        {".vss", "application/vnd.visio"},
        {".vsscc", "text/plain"},
        {".vssettings", "text/xml"},
        {".vssscc", "text/plain"},
        {".vst", "application/vnd.visio"},
        {".vstemplate", "text/xml"},
        {".vsto", "application/x-ms-vsto"},
        {".vsw", "application/vnd.visio"},
        {".vsx", "application/vnd.visio"},
        {".vtx", "application/vnd.visio"},
        {".wav", "audio/wav"},
        {".wave", "audio/wav"},
        {".wax", "audio/x-ms-wax"},
        {".wbk", "application/msword"},
        {".wbmp", "image/vnd.wap.wbmp"},
        {".wcm", "application/vnd.ms-works"},
        {".wdb", "application/vnd.ms-works"},
        {".wdp", "image/vnd.ms-photo"},
        {".webarchive", "application/x-safari-webarchive"},
        {".webtest", "application/xml"},
        {".wiq", "application/xml"},
        {".wiz", "application/msword"},
        {".wks", "application/vnd.ms-works"},
        {".WLMP", "application/wlmoviemaker"},
        {".wlpginstall", "application/x-wlpg-detect"},
        {".wlpginstall3", "application/x-wlpg3-detect"},
        {".wm", "video/x-ms-wm"},
        {".wma", "audio/x-ms-wma"},
        {".wmd", "application/x-ms-wmd"},
        {".wmf", "application/x-msmetafile"},
        {".wml", "text/vnd.wap.wml"},
        {".wmlc", "application/vnd.wap.wmlc"},
        {".wmls", "text/vnd.wap.wmlscript"},
        {".wmlsc", "application/vnd.wap.wmlscriptc"},
        {".wmp", "video/x-ms-wmp"},
        {".wmv", "video/x-ms-wmv"},
        {".wmx", "video/x-ms-wmx"},
        {".wmz", "application/x-ms-wmz"},
        {".wpl", "application/vnd.ms-wpl"},
        {".wps", "application/vnd.ms-works"},
        {".wri", "application/x-mswrite"},
        {".wrl", "x-world/x-vrml"},
        {".wrz", "x-world/x-vrml"},
        {".wsc", "text/scriptlet"},
        {".wsdl", "text/xml"},
        {".wvx", "video/x-ms-wvx"},
        {".x", "application/directx"},
        {".xaf", "x-world/x-vrml"},
        {".xaml", "application/xaml+xml"},
        {".xap", "application/x-silverlight-app"},
        {".xbap", "application/x-ms-xbap"},
        {".xbm", "image/x-xbitmap"},
        {".xdr", "text/plain"},
        {".xht", "application/xhtml+xml"},
        {".xhtml", "application/xhtml+xml"},
        {".xla", "application/vnd.ms-excel"},
        {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
        {".xlc", "application/vnd.ms-excel"},
        {".xld", "application/vnd.ms-excel"},
        {".xlk", "application/vnd.ms-excel"},
        {".xll", "application/vnd.ms-excel"},
        {".xlm", "application/vnd.ms-excel"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
        {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".xlt", "application/vnd.ms-excel"},
        {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
        {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
        {".xlw", "application/vnd.ms-excel"},
        {".xml", "text/xml"},
        {".xmta", "application/xml"},
        {".xof", "x-world/x-vrml"},
        {".XOML", "text/plain"},
        {".xpm", "image/x-xpixmap"},
        {".xps", "application/vnd.ms-xpsdocument"},
        {".xrm-ms", "text/xml"},
        {".xsc", "application/xml"},
        {".xsd", "text/xml"},
        {".xsf", "text/xml"},
        {".xsl", "text/xml"},
        {".xslt", "text/xml"},
        {".xsn", "application/octet-stream"},
        {".xss", "application/xml"},
        {".xtp", "application/octet-stream"},
        {".xwd", "image/x-xwindowdump"},
        {".z", "application/x-compress"},
        {".zip", "application/x-zip-compressed"},
        #endregion

        };

        public static string GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            string mime;

            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }

        public static string UploadImageFromFileStream(Stream resizedImage, string filename, CloudBlobContainer container)
        {
            string extension = Path.GetExtension(filename);
            string mimeType = GetMimeType(extension);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            if (!blockBlob.Exists())
            {
                blockBlob.Properties.CacheControl = "public, max-age=31536000";
                if (!string.IsNullOrEmpty(mimeType))
                    blockBlob.Properties.ContentType = mimeType;

                resizedImage.Seek(0, SeekOrigin.Begin);
                blockBlob.UploadFromStream(resizedImage);
            }

            return filename;
        }

        public static Stream FetchDocument(PictureType pictureType, string filename)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(pictureType.ToString().ToLowerInvariant());
            if (!container.Exists())
                container.Create(BlobContainerPublicAccessType.Blob);

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

            MemoryStream memoryStream = new MemoryStream();
            blockBlob.DownloadToStream(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;

        }

        public static void DeleteBlob(PictureType pictureType, string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(pictureType.ToString().ToLowerInvariant());
                if (!container.Exists())
                    container.Create(BlobContainerPublicAccessType.Blob);


                CloudBlockBlob blob = container.GetBlockBlobReference(filename);
                blob.DeleteIfExists();

                if (IsValidImage(filename))
                {
                    string thumbname = GetThumbName(filename);
                    if (!string.IsNullOrEmpty(thumbname))
                    {
                        CloudBlockBlob thumbBlob = container.GetBlockBlobReference(thumbname);
                        thumbBlob.DeleteIfExists();
                    }
                }
            }
        }

        public static void DownloadBlob(PictureType pictureType, string filename)
        {
            string strExtension = Path.GetExtension(filename).ToLower();
            string mimeType = GetMimeType(strExtension);

            string dest = Path.Combine(HttpRuntime.CodegenDir, filename);
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(pictureType.ToString().ToLowerInvariant());
            try
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
                // Save blob contents to a file.
                using (var fileStream = File.OpenWrite(dest))
                {
                    blockBlob.DownloadToStream(fileStream);
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=\"" + filename + "\"");
                HttpContext.Current.Response.ContentType = mimeType;
                HttpContext.Current.Response.TransmitFile(dest);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
            // Clean up temporary file.
            File.Delete(dest);
        }

        public static string GetThumbName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && (Path.GetExtension(fileName) == ".jpg" || Path.GetExtension(fileName) == ".png" || Path.GetExtension(fileName) == ".jpeg"))
            {
                var extension = Path.GetExtension(fileName);
                if (!string.IsNullOrEmpty(extension))
                {
                    var thumbName = string.Format("{0}-thumb{1}", fileName.Replace(extension, string.Empty), extension.ToLower());
                    return thumbName;
                }
            }
            return string.Empty;
        }

        public static string GetWebConfigKeyValue(string keyNmae)
        {
            return ConfigurationManager.AppSettings[keyNmae];
        }

        private static readonly string[] _imageFiles = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        public static bool IsValidImage(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            return true;// _imageFiles.Contains(extension, StringComparer.InvariantCultureIgnoreCase);
        }

        public static bool IsFileExists(string fileName, PictureType pictureType)
        {
            string dirPath = string.Empty;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            dirPath = pictureType.ToString().ToLowerInvariant();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(dirPath);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            if (blockBlob != null)
            {
                if (!blockBlob.Exists())
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }
        public static void CreateBlobContainers(string Host)
        {

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(Host.ToString().ToLowerInvariant());
            if (!container.Exists())
                container.Create(BlobContainerPublicAccessType.Blob);

            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

        }
        public static void UploadToBlobStorage(FileInfo zipFile, string blobContainerName)
        {
            // Connect to the storage account's blob endpoint 
            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();

            // Create the blob storage container 
            CloudBlobContainer container = client.GetContainerReference("exportdata");
            container.CreateIfNotExists();

            // Create the blob in the container 
            CloudBlockBlob blob = container.GetBlockBlobReference(blobContainerName + "/" + zipFile.Name);

            // Upload the zip and store it in the blob 
            using (FileStream fs = zipFile.OpenRead())
                blob.UploadFromStream(fs);
        }
        public static string GetFileURLByDomain(string DomainName, string FileName)
        {
            if (!string.IsNullOrWhiteSpace(FileName))
                return GetWebConfigKeyValue("AzureStoragePath") + DomainName.ToString().ToLowerInvariant() + "/" + FileName;
            else
                return string.Empty;
        }

        public static void DownloadBlobZip(string pictureType, string filename)
        {
            string strExtension = Path.GetExtension(filename).ToLower();
            string mimeType = GetMimeType(strExtension);

            string dest = Path.Combine(HttpRuntime.CodegenDir, filename);
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(pictureType.ToString().ToLowerInvariant());
            try
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
                // Save blob contents to a file.
                using (var fileStream = File.OpenWrite(dest))
                {
                    blockBlob.DownloadToStream(fileStream);
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=\"" + filename + "\"");
                HttpContext.Current.Response.ContentType = mimeType;
                HttpContext.Current.Response.TransmitFile(dest);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
            // Clean up temporary file.
            File.Delete(dest);
        }
        public static void DeleteBlobFile(string pictureType, string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(pictureType.ToString().ToLowerInvariant());
                if (!container.Exists())
                    container.Create(BlobContainerPublicAccessType.Blob);


                CloudBlockBlob blob = container.GetBlockBlobReference(filename);
                blob.DeleteIfExists();

                //if (IsValidImage(filename))
                //{
                //    string thumbname = GetThumbName(filename);
                //    if (!string.IsNullOrEmpty(thumbname))
                //    {
                //        CloudBlockBlob thumbBlob = container.GetBlockBlobReference(thumbname);
                //        thumbBlob.DeleteIfExists();
                //    }
                //}
            }
        }

        public static void UploadImportDataToBlobStorage(FileInfo zipFile, string blobContainerName)
        {
            // Connect to the storage account's blob endpoint 
            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();

            // Create the blob storage container 
            CloudBlobContainer container = client.GetContainerReference("importdata");
            container.CreateIfNotExists();

            // Create the blob in the container 
            CloudBlockBlob blob = container.GetBlockBlobReference(blobContainerName + "/" + zipFile.Name);

            // Upload the zip and store it in the blob 
            using (FileStream fs = zipFile.OpenRead())
                blob.UploadFromStream(fs);
        }

        public static int CreateBlobAndUploadImportFile(string Host, HttpPostedFileBase file, string fileName)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("importdatafile");
            if (!container.Exists())
                container.Create(BlobContainerPublicAccessType.Blob);

            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

            CloudBlockBlob blob = container.GetBlockBlobReference(Host + "/" + fileName);
            blob.UploadFromStreamAsync(file.InputStream);
            return 0;
        }
    }
}

