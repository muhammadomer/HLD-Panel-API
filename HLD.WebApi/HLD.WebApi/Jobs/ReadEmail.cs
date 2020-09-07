using Newtonsoft.Json;
using OpenPop.Mime;
using OpenPop.Pop3;
using Quartz;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class ReadEmail : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
               
                using (ImapClient client = new ImapClient("imap.mail.us-east-1.awsapps.com", 993, "info@hldinc.net", "w0rkmail897?>", AuthMethod.Login, true))
                {
                    // Returns a collection of identifiers of all mails matching the specified search criteria.
                    IEnumerable<uint> uids = client.Search(SearchCondition.From("habib.rehman@phenologix.com"));
                    // Download mail messages from the default mailbox.
                    IEnumerable<MailMessage> messages = client.GetMessages(uids);
                    foreach(var item in messages)
                        {
                    

                       

                    }
                }
            }
            catch (Exception ex )
            {

                throw;
            }

          //  FetchAllMessages("imap.mail.us-east-1.awsapps.com", 993, false , "amz_ca2", "ylKB9bUCvZsPGikdbis1");


            await Task.CompletedTask;

        }
     

public void addMessage(string message, string header)
        {
            string full_body = header + "\n" + message;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            Byte[] full_body_bytes = encoding.GetBytes(full_body);
            Message mm = new Message(full_body_bytes);

            //do stuff here.
        }
    }
}
