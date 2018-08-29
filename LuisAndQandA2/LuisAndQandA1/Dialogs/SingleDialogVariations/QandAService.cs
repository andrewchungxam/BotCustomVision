using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace LuisAndQandA1.Dialogs
{
    //HELPER CLASSES
    public class RootObject
    {
        public string question { get; set; }
    }

    public class Metadata { public string Name { get; set; } public string Value { get; set; } }

    public class Answer { public IList<string> Questions { get; set; } public string answer { get; set; } public double Score { get; set; } public int Id { get; set; } public string Source { get; set; } public IList<object> Keywords { get; set; } public IList<Metadata> Metadata { get; set; } }

    public class QnAAnswer { public IList<Answer> Answers { get; set; } }


    [Serializable]
    public class QandAService
    {
        private string _qandaServiceHostName;
        private string _knowledgeBaseId;
        private string _endpointKey;

        //CREATE THE Q AND A DIALOG
        public QandAService(string hostname, string knowledgeBasedId, string endpointKey)
        {
            _qandaServiceHostName = hostname;
            _knowledgeBaseId = knowledgeBasedId;
            _endpointKey = endpointKey;
        }

        //CALL THE Q AND A ENDPOINT AND GET A RESPONSE
        public async Task<string> GetAnswer(string query)
        {
            var client = new HttpClient()
            {
            };

            string baseUrlString = String.Format("{0}/knowledgebases/{1}/generateAnswer", _qandaServiceHostName, _knowledgeBaseId);
            var request = new HttpRequestMessage(HttpMethod.Post, baseUrlString);

            //ADD QUERY IN REQUEST MESSAGE BODY
            var requestQueryQandAObject = new RootObject()
            {
                question = query
            };
            
            //SERIALIZE THE REQUEST MESSAGE
            string serializedRequestQueryQandAObject = JsonConvert.SerializeObject(requestQueryQandAObject);
            request.Content = new StringContent(serializedRequestQueryQandAObject, Encoding.UTF8, "application/json");

            request.Headers.Add("Authorization", "EndpointKey " + _endpointKey);

            //RETRIEVE RESPONSE MESSAGE FROM ABOVE RESPONSE
            var response = await client.SendAsync(request);

            //var responseResponse = await response;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                //EXTRACT THE RESPONSE FROM HTTP RESPONSE MESSAGE
                var responseContentAsString = response.Content.ReadAsStringAsync().Result;

                // Deserialize the response JSON                 
                QnAAnswer answer = JsonConvert.DeserializeObject<QnAAnswer>(responseContentAsString);

                // Return the answer if present                 
                if (answer.Answers.Count > 0)
                {
                    if(answer.Answers[0].answer != "No good match found in KB.")
                    {
                        return answer.Answers[0].answer;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                throw new Exception
                    ($"QnAMaker call failed with status code {response.StatusCode.ToString()} { response.ReasonPhrase }");            
            }
        }
    }
}





