using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Vision;
using ImageCaption.Services;
using Microsoft.Cognitive.CustomVision.Prediction;
using System.Text;

using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LuisAndQandA1.Services;
using ZXing;
using ZXing.Common;
using System.Drawing;

namespace LuisAndQandA1.Dialogs
{
    public class BarcodeScannerService
    {
        private readonly IBarcodeReader barcodeReader;

        public BarcodeScannerService()
        {
            barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true
                }
            };
        }

        //        private void DecodeBarcode(byte[] byteArrayFile)
        public String DecodeBarcode(byte[] byteArrayFile)
        {

            var newStringBuilder = new StringBuilder();

            try
            {
                //using (var bitmap = (Bitmap)Bitmap.FromFile(barcodeImageFile))
                //{
                    try
                    {
                        Bitmap bmp;
                        using (var ms = new MemoryStream(byteArrayFile))
                        {
                            bmp = new Bitmap(ms);
                        }

                        var result = barcodeReader.Decode(bmp);
                        bmp.Dispose();

                        //var result = barcodeReader.Decode(bitmap);
                        //Console.WriteLine("Result: {0}", result.Text);
                        //resultWriter.WriteLine("RESULT:{0}", result.Text);
                        //resultWriter.WriteLine("FORMAT: {0}", result.BarcodeFormat);

                        foreach (var metaData in result.ResultMetadata)
                        {
                            newStringBuilder.Append($"Metadata:{metaData.Key}: {metaData.Value}");
                            newStringBuilder.Append(Environment.NewLine);
                            newStringBuilder.Append($"Barcode Format: {result.BarcodeFormat.ToString()}");
                            newStringBuilder.Append(Environment.NewLine);
                            newStringBuilder.Append($"Text:{result.ToString()}");
                            //resultWriter.WriteLine("METADATA:{0}:{1}", metaData.Key, metaData.Value);
                    }

                    //result.Text;


                    //if (result != null)
                    //{


                    //}



                        return newStringBuilder.ToString();

                    }
                    catch (Exception innerExc)
                    {
                        Console.WriteLine("Exception: {0}", innerExc.Message);
                        return "Inner Execption";

                        //resultWriter.Write(innerExc.ToString());
                    }
                //}
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception: {0}", exc.Message);
                return "Exception";
            }
        }

        //private void DecodeBarcode(string fileName)
        //{
        //    try
        //    {
        //        //Console.WriteLine("Decoding image: {0}", fileName);

        //        //var barcodeImageFile = fileName;
        //        //var barcodeResultFile = fileName + ".txt";
        //        //using (var resultWriter = new StreamWriter(barcodeResultFile, false, Encoding.UTF8))



        //        using (var bitmap = (Bitmap)Bitmap.FromFile(barcodeImageFile))
        //        {
        //            try
        //            {
        //                var result = barcodeReader.Decode(bitmap);

        //                Console.WriteLine("Result: {0}", result.Text);

        //                resultWriter.WriteLine("RESULT:{0}", result.Text);
        //                resultWriter.WriteLine("FORMAT: {0}", result.BarcodeFormat);
        //                foreach (var metaData in result.ResultMetadata)
        //                {
        //                    resultWriter.WriteLine("METADATA:{0}:{1}", metaData.Key, metaData.Value);
        //                }
        //            }
        //            catch (Exception innerExc)
        //            {
        //                Console.WriteLine("Exception: {0}", innerExc.Message);

        //                resultWriter.Write(innerExc.ToString());
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        Console.WriteLine("Exception: {0}", exc.Message);
        //    }
        //}
    }

        [Serializable]
    public class SimpleZXingBarcodeDialog : IDialog<string>
    {
        private readonly ICaptionService captionService = new MicrosoftCognitiveCaptionService();
        private int attempts = 3;
        private string itemType = "headphones";


        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Post an image of your {itemType}!");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var imageAttachment = activity.Attachments?.FirstOrDefault(a => a.ContentType.Contains("image"));
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            //PredictionEndpoint endpoint = new PredictionEndpoint() { ApiKey = predictionKey };

            if (imageAttachment != null)
            {
                //context.PostAsync($"Receiving image...");

                //VERSION 1 -23 seconds
                //using (var stream = await GetImageStream(connector, imageAttachment))
                //{
                //byte[] sampleByteArray = await GetImageAsByteArrayFromStream(stream);
                //END VERSION 1

                //VERSION 2
                byte[] sampleByteArray = await GetImageByteStreamDirectly(connector, imageAttachment);
                context.PostAsync($"Received image, transcribing images into text...");

                var bcss = new BarcodeScannerService();
                var returnedString = bcss.DecodeBarcode(sampleByteArray);

                await context.PostAsync($"Returned Barcode Scanner: {returnedString}");
                
                

                //{
                //END VERSION 2

                // Replace <Subscription Key> with your valid subscription key.
                //const string subscriptionKey = "<Subscription Key>";
                const string subscriptionKey = "9ed71cc74b73452aac3fa36f6c23f7e6";

                //< add key = "MicrosoftVisionApiEndpoint" value = "https://westus.api.cognitive.microsoft.com/vision/v1.0" />
                //   < add key = "MicrosoftVisionApiKey" value = "9ed71cc74b73452aac3fa36f6c23f7e6" />

                // You must use the same region in your REST call as you used to
                // get your subscription keys. For example, if you got your
                // subscription keys from westus, replace "westcentralus" in the URL
                // below with "westus".
                //
                // Free trial subscription keys are generated in the westcentralus region.
                // If you use a free trial subscription key, you shouldn't need to change
                // this region.
                //const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/ocr";
                const string uriBase = "https://westus.api.cognitive.microsoft.com/vision/v2.0/ocr";


                try
                {
                    HttpClient client = new HttpClient();

                    // Request headers.
                    client.DefaultRequestHeaders.Add(
                        "Ocp-Apim-Subscription-Key", subscriptionKey);

                    // Request parameters.
                    string requestParameters = "language=unk&detectOrientation=true";

                    // Assemble the URI for the REST API Call.
                    string uri = uriBase + "?" + requestParameters;

                    HttpResponseMessage response;

                    // Request body. Posts a locally stored JPEG image.
                    //byte[] byteData = GetImageAsByteArray(imageFilePath);

                    //using (ByteArrayContent content = new ByteArrayContent(byteData))
                    using (ByteArrayContent content = new ByteArrayContent(sampleByteArray))
                    {
                        // This example uses content type "application/octet-stream".
                        // The other content types you can use are "application/json"
                        // and "multipart/form-data".
                        content.Headers.ContentType =
                            new MediaTypeHeaderValue("application/octet-stream");

                        // Make the REST API call.
                        response = await client.PostAsync(uri, content);

                        //            //DESERIALIZE RESPONSE FROM HTTP RESPONSE MESSAGE(JSON->OBJECT)
                        //var myPostCreateAccessPolicyresultObject2 =
                        //    Newtonsoft.Json.JsonConvert.DeserializeObject<XamCamFunctions.DataModels.CreateAccessPolicy.RootObject>(myPostCreateAccessPolicystringResult2);

                        //            var myPostCreateAccessPolicydObjectResults2 = myPostCreateAccessPolicyresultObject2.d;
                        //            var myPostCreateAccessPolicyaccessPolicyIdResults2 = myPostCreateAccessPolicydObjectResults2.Id;
                    }

                    string contentString = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = Newtonsoft.Json.JsonConvert
                                .DeserializeObject<OCRRootObject>(contentString);

                    StringBuilder deseralizedResponseToString = new StringBuilder();

                    if (deserializedResponse != null && deserializedResponse.regions != null)
                    {
                        //deseralizedResponseToString.Append("Text: ");
                        //deseralizedResponseToString.AppendLine();
                        foreach (var item in deserializedResponse.regions)
                        {
                            foreach (var line in item.lines)
                            {
                                foreach (var word in line.words)
                                {
                                    deseralizedResponseToString.Append(word.text);
                                    deseralizedResponseToString.Append(" ");
                                }
                                deseralizedResponseToString.AppendLine();
                            }
                            deseralizedResponseToString.AppendLine();
                        }
                    }
                    //context.PostAsync($"Azure OCR results: { JToken.Parse(contentString).ToString()}");  

                    await context.PostAsync($"Azure OCR results:    {deseralizedResponseToString}");

                    // Get the JSON response.
                    //string contentString = await response.Content.ReadAsStringAsync();

                    // Display the JSON response.
                    //Console.WriteLine("\nResponse:\n\n{0}\n", JToken.Parse(contentString).ToString());
                    //    await context.PostAsync($"Azure OCR results: { JToken.Parse(contentString).ToString()}");

                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e.Message);
                }

                context.Wait(MessageReceivedAsync);


                //var predictionImageResult = endpoint.PredictImage(new Guid(predictionProjectId), stream);
                //StringBuilder predictionImageResultString = new StringBuilder();

                //var resultsList = new List<PredictionResult>();

                //foreach (var c in predictionImageResult.Predictions)
                //{

                //    if (c.Probability > .50)
                //    {
                //        predictionImageResultString.Append(c.Tag);
                //        predictionImageResultString.Append(", ");
                //        var probability = (decimal)c.Probability * 100;
                //        var probabilityFixed = probability.ToString("#.##");
                //        predictionImageResultString.Append(probabilityFixed);
                //        predictionImageResultString.Append("%");
                //        predictionImageResultString.Append("; ");
                //    }
                //    //resultsDictionary.Add(c.Tag, (decimal)c.Probability);
                //    //resultsList.Add(new PredictionResult() { PredictionTag = c.Tag, PredictionProbability = (decimal)c.Probability});
                //}

                //await context.PostAsync($"Azure Custom Vision results: {predictionImageResultString}");

                // var resultsDictionaryString = resultsDictionary.ToString();
                // await context.PostAsync($"{resultsDictionaryString}");

                //var resultsListToString = String.Join<PredictionResult>(", ", resultsList.ToArray());
                //await context.PostAsync($"{resultsListToString}");

                //
                //MAIN
                //

                //if (imageAttachment != null)
                //{
                //    using (var stream = await GetImageStream(connector, imageAttachment))
                //    {
                //        //return await this.captionService.GetCaptionAsync(stream);
                //        var textOfStream = await this.captionService.GetCaptionAsync(stream);
                //        await context.PostAsync($"{textOfStream}");
                //    }
                //}
                ////ELSE --??
                //context.Wait(MessageReceivedAsync);


                //string predictionProjectId = "3e152a33-92ee-4326-b4c9-c8068edab9d8"; //"YOUR_PROJECT_ID", //This is the custom vision project we are predicting against
                //string predictionKey = "eafbd7b247bd41c1afd6d6da352f7099";

                //PredictionEndpoint endpoint = new PredictionEndpoint() { ApiKey = predictionKey };

                //if (imageAttachment != null)
                //{
                //    using (var stream = await GetImageStream(connector, imageAttachment))
                //    {
                //        var predictionImageResult = endpoint.PredictImage(new Guid(predictionProjectId), stream);
                //        StringBuilder predictionImageResultString = new StringBuilder();

                //        var resultsList = new List<PredictionResult>();

                //        foreach (var c in predictionImageResult.Predictions)
                //        {

                //            if (c.Probability > .50)
                //            {
                //                predictionImageResultString.Append(c.Tag);
                //                predictionImageResultString.Append(", ");
                //                var probability = (decimal)c.Probability * 100;
                //                var probabilityFixed = probability.ToString("#.##");
                //                predictionImageResultString.Append(probabilityFixed);
                //                predictionImageResultString.Append("%");
                //                predictionImageResultString.Append("; ");
                //            }
                //            //resultsDictionary.Add(c.Tag, (decimal)c.Probability);
                //            //resultsList.Add(new PredictionResult() { PredictionTag = c.Tag, PredictionProbability = (decimal)c.Probability});
                //        }

                //        await context.PostAsync($"Azure Custom Vision results: {predictionImageResultString}");

                //        // var resultsDictionaryString = resultsDictionary.ToString();
                //        // await context.PostAsync($"{resultsDictionaryString}");

                //        var resultsListToString = String.Join<PredictionResult>(", ", resultsList.ToArray());
                //        //await context.PostAsync($"{resultsListToString}");

                //}
                //    }

                //context.Wait(MessageReceivedAsync);

                //if (imageAttachment != null)
                //{
                //    using (var stream = await GetImageStream(connector, imageAttachment))
                //    {
                //        //return await this.captionService.GetCaptionAsync(stream);
                //        var textOfStream = await this.captionService.GetCaptionAsync(stream);
                //        await context.PostAsync($"{textOfStream}");
                //    }
                //}

                //}
            }
        }

        private static async Task<Stream> GetImageStream(ConnectorClient connector, Attachment imageAttachment)
        {
            using (var httpClient = new HttpClient())
            {
                // The Skype attachment URLs are secured by JwtToken,
                // you should set the JwtToken of your bot as the authorization header for the GET request your bot initiates to fetch the image.
                // https://github.com/Microsoft/BotBuilder/issues/662
                var uri = new Uri(imageAttachment.ContentUrl);
                if (uri.Host.EndsWith("skype.com") && uri.Scheme == "https")
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync(connector));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                }

                return await httpClient.GetStreamAsync(uri);
            }
        }

        private static async Task<byte[]> GetImageByteStreamDirectly(ConnectorClient connector, Attachment imageAttachment)
        {
            using (var httpClient = new HttpClient())
            {
                // The Skype attachment URLs are secured by JwtToken,
                // you should set the JwtToken of your bot as the authorization header for the GET request your bot initiates to fetch the image.
                // https://github.com/Microsoft/BotBuilder/issues/662
                var uri = new Uri(imageAttachment.ContentUrl);
                if (uri.Host.EndsWith("skype.com") && uri.Scheme == "https")
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync(connector));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                }

                return await httpClient.GetByteArrayAsync(uri);
            }
        }

        private static async Task<byte[]> GetImageAsByteArrayFromStream(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }


        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArrayFromFile(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }


        /// <summary>
        /// Gets the JwT token of the bot. 
        /// </summary>
        /// <param name="connector"></param>
        /// <returns>JwT token of the bot</returns>
        private static async Task<string> GetTokenAsync(ConnectorClient connector)
        {
            var credentials = connector.Credentials as MicrosoftAppCredentials;
            if (credentials != null)
            {
                return await credentials.GetTokenAsync();
            }

            return null;
        }

    }
}

