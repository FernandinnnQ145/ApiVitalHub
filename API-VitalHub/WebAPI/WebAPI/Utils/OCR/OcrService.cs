using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace WebAPI.Utils.OCR
{
    public class OcrService
    {
        private readonly string _subscriptKey = "ffff20b4e4ab40c39bf8a29902baa871";

        private readonly string _endpoint = "https://cvvitalhubfernando.cognitiveservices.azure.com/";

        public async Task<string> RecognizeTextAsync(Stream imageStream)
        {
            try
            {
                var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subscriptKey))
                {
                    Endpoint = _endpoint
                };

                var ocrResult = await client.RecognizePrintedTextInStreamAsync(true, imageStream);

                return ProcessRecognitionResult(ocrResult);
            }
            catch (Exception ex) 
            {

                return "Erro ao reconhecer o texto: " + ex.Message;
            }
        }

        private static string ProcessRecognitionResult(OcrResult result)
        {
            try
            {
                string recognizadText = "";

                foreach (var region in result.Regions)
                {
                    foreach(var line in region.Lines)
                    {
                        foreach(var word in line.Words)
                        {
                            recognizadText += word.Text + " ";
                        }
                        recognizadText+= "\n";
                    }
                }
                return recognizadText;

                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
