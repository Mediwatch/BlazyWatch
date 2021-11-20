using System;
using PayPalCheckoutSdk.Core;

using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace PayPal
{
    public class PayPalClient
    {
        // Place these static properties into a settings area.
        public static string SandboxClientId { get; set; } = "AS60MoczPXk40sTYAtzCr41h2BgSmUNk4RxhPndZ-Ajz0Ic7hKQRWMd7JWNtB-OhT0C55MtweuBTSJmF";
        public static string SandboxClientSecret { get; set; } = "EPqnAd3wjI4-O9Jlq3z4TeiRElVU_qTBPGKOZAEUN_3ngf7YWJrvm1xrv2U_WcpflhgmT3_paPYUhwHu";

        public static string LiveClientId { get; set; } = "AZItd4zy21X2GzYkiEOYYZY_9IeuD-rLCkVZwFZZ5HxvWF8Q8HZapFkb1zNdpMREbF7ojvTEEy1el71J";
        public static string LiveClientSecret { get; set; } = "EF51AHws8qrjIeEtweN05qJyEtePI-BIuU23z-gyrg8kQN7QsHdplkIK4G6y7RSiZhbB4gjbWe4TNboB";



        ///<summary>
        /// Set up PayPal environment with sandbox credentials.
        /// In production, use LiveEnvironment.
        ///</summary>
        public static PayPalEnvironment Environment()
        {
#if DEBUG
            // You may want to create a UAT (user exceptance tester) role and check for this:
            // "if(_unitOfWork.IsUATTester(GetUserId())" instead of fcomiler directives.
            return new SandboxEnvironment(SandboxClientId,
                                          SandboxClientSecret);
#else
            return new LiveEnvironment(LiveClientId, 
                                       LiveClientSecret);
#endif
        }

        ///<summary>
        /// Returns PayPalHttpClient instance to invoke PayPal APIs.
        ///</summary>
        public static PayPalCheckoutSdk.Core.PayPalHttpClient Client()
        {
            return new PayPalHttpClient(Environment());
        }

        public static PayPalCheckoutSdk.Core.PayPalHttpClient Client(string refreshToken)
        {
            return new PayPalHttpClient(Environment(), refreshToken);
        }


        ///<summary>
        /// Use this method to serialize Object to a JSON string.
        ///</summary>
        public static String ObjectToJSONString(Object serializableObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            var writer = JsonReaderWriterFactory.CreateJsonWriter(memoryStream,
                                                                  Encoding.UTF8,
                                                                  true,
                                                                  true,
                                                                  "  ");

            var ser = new DataContractJsonSerializer(serializableObject.GetType(),
                                                     new DataContractJsonSerializerSettings
                                                     {
                                                         UseSimpleDictionaryFormat = true
                                                     });

            ser.WriteObject(writer,
                            serializableObject);

            memoryStream.Position = 0;
            StreamReader sr = new StreamReader(memoryStream);

            return sr.ReadToEnd();
        }
    }
}