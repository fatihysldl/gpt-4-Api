using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace gpt4Api
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("GPT-4 AI'ye hoş geldiniz! Çıkmak için 'exit' yazabilirsiniz.");

            
            string baseUrl = "https://openrouter.ai/api/v1/chat/completions";
            string apiKey = "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"; 

            while (true)
            {
                // Kullanıcıdan soru al
                Console.Write("\n Soru: ");
                string userQuestion = Console.ReadLine();

                // Çıkış kontrolü
                if (userQuestion.ToLower() == "exit")
                {
                    Console.WriteLine("Uygulamadan çıkılıyor...");
                    break;
                }

                // API çağrısı yap ve yanıt al
                string response = await askGpt4(baseUrl, apiKey, userQuestion);

                // Yanıtı yazdır
                Console.WriteLine("\n Cevap: " + response);
            }
        }

        static async Task<string> askGpt4(string baseUrl, string apiKey, string userQuestion)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                    // İstek verisini hazırla
                    var requestBody = new
                    {
                        model = "deepseek/deepseek-chat:free",
                        messages = new[]
                        {
                            new { role = "user", content = userQuestion }
                        }
                    };

                    // JSON formatına dönüştür
                    string jsonBody = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    // POST isteği gönder
                    HttpResponseMessage response = await client.PostAsync(baseUrl, content);
                    response.EnsureSuccessStatusCode();

                    // Yanıtı işle
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseContent);

                    // AI yanıtını döndür
                    return result.choices[0].message.content;
                }
            }
            catch (Exception ex)
            {
                return "Hata: " + ex.Message;
            }
        }
    }
}
