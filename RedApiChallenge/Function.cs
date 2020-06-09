using System;
using System.Collections.Generic;
using System.Net;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace RedApiChallenge
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {

            APIGatewayProxyResponse response = new APIGatewayProxyResponse();

            string route = input.Path.ToLower();

            try
            {
                switch (route)
                {
                    case "/api/fibonacci":
                        var fibonacciNumber = FibonacciNumber(input.QueryStringParameters["n"]);
                        response = CreateResponse(fibonacciNumber.ToString());
                        break;
                    case "/api/reversewords":
                        var reverseWords = ReverseWords(input.QueryStringParameters["sentence"]);
                        response = CreateResponse(reverseWords);
                        break;
                    case "/api/token":
                        response = CreateResponse("8cc33dc3-5f8f-49fe-af8a-7ae4fdab3141");
                        break;
                    case "/api/triangletype":
                        var triangleType = TriangleType(
                                checked(int.Parse(input.QueryStringParameters["a"])),
                                checked(int.Parse(input.QueryStringParameters["b"])),
                                checked(int.Parse(input.QueryStringParameters["c"])));
                        response = CreateResponse(triangleType);
                        break;
                    default:
                        response = CreateResponse("error : Api Unknown");
                        break;
                }
            }
            catch (OverflowException) 
            {
                response = CreateResponse(null);
            }
            catch (Exception e)
            {
                response = CreateResponse("error : " + e.Message);
            }

            return response;
        }
        
        private long FibonacciNumber(string input ) 
        {
            long n;
            bool negativeInput = false;
            long Fx = 0;
            long Fx_1 = 1;
            long Fx_2 = -1;

            if (input.Contains("-"))
            {
                negativeInput = true;
                input = input.Replace("-", string.Empty);    
            }

            n = long.Parse(input, System.Globalization.NumberStyles.Number);
            
            while (n >= 0) 
            {                
                Fx = checked(Fx_1 + Fx_2);
                Fx_2 = Fx_1;
                Fx_1 = Fx;
                n--;
            }

            if (negativeInput)
            {
                Fx *=-1;
            }       
            
            return Fx;
        }


        private string ReverseWords(string sentence) 
        {
            string reverse = "";
            string[] words = sentence.Split(' ', StringSplitOptions.None);
            int wordCounter = 0;

            foreach (var word in words)
            {
                for (int i = word.Length; i > 0; i--)
                {
                    reverse += word[i - 1];
                }
                wordCounter++;

                if (wordCounter < words.Length)
                {
                    reverse += " ";
                }
                
            }

            return reverse; 
        }


        private string TriangleType(int a, int b, int c) 
        {
            string type;

            //if (c == Math.Sqrt((a * a) + (b * b)))
            //{
            //    type = "Right Angle";
            //}
            //else 
            if ((a + b <= c) || (a + c <= b) || (c + b <= a))
            {
                type = "Error";
            }
            else if ((a == b) && (b == c))
            {
                type = "Equilateral";
            }
            else if (a == b || a == c || b == c)
            {
                type = "Isosceles";
            }
            else
            {
                type = "Scalene";
            }

            return type; 

        }

        private APIGatewayProxyResponse CreateResponse(string result)
        {
            int statusCode = (result != null && !result.Contains("error")) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.BadRequest;

            string body = result ?? string.Empty;

            var response = new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = body,
                Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" },
                        { "Access-Control-Allow-Origin", "*" }
                    }
            };

            return response;
        }


    }
}
