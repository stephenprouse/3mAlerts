using System;
using System.Threading;
using System.IO;
using System.Net.Mail;

namespace _3M_Alerts
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime now = DateTime.Now;
            try
            {
                bool adtok = true;
                bool chgok = true;

                DateTime adtFiledt = File.GetLastWriteTime(@"\\uch3m02\3mhis\CONNPLUS\data\adt.in");
                DateTime chgFiledt = File.GetLastWriteTime(@"\\uch3m02\LIVE_CHG\chg.bak");
                string mailBody = "";

                if (chgFiledt < now.AddHours(-26))
                {
                    Console.WriteLine("chg.bak file is too old! " + chgFiledt.ToString());
                    mailBody = "chg.bak file is too old! " + chgFiledt.ToString();
                    chgok = false;
                }
                else
                {
                    Console.WriteLine("chg.bak file is just right. " + chgFiledt.ToString());
                }

                if (adtFiledt < now.AddHours(-1))
                {
                    Console.WriteLine("adt.in file is too old! " + adtFiledt.ToString());
                    if (chgok == false)
                    {
                        mailBody = mailBody + "\r\nadt.in file is too old! " + adtFiledt.ToString();
                    }
                    else
                    {
                        mailBody = "adt.in file is too old! " + adtFiledt.ToString();
                    }
                    adtok = false;
                }
                else
                {
                    Console.WriteLine("adt.in file is just right. " + adtFiledt.ToString());
                }

                //send an email or not
                if (adtok == false || chgok == false)
                {
                    SendEmailMsg(mailBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            
            Console.WriteLine("\r\nPausing for 5 seconds");
            Thread.Sleep(5000);
        }

        static void SendEmailMsg(string mailBody)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("uchexchangehmh");
                mail.Priority = MailPriority.High;
                mail.From = new MailAddress("3Malert@uchs.org");
                mail.To.Add("mbohlen@uchs.org");
                mail.To.Add("sprouse@uchs.org");
                mail.To.Add("lmitzel@uchs.org");
                mail.To.Add("BPhillips@uchs.org");
                mail.To.Add("CLannen@uchs.org");
                mail.Subject = "3M Alert";
                mail.Body = mailBody;
                SmtpServer.Port = 25;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}