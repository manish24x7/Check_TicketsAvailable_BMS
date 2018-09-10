using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Drawing.Imaging;
using System.Net.Mail;

namespace BMS_Check_MovieTicket
{
    class BMS
    {
        public static void BookTickets(string MovieName, List<string> PreferredTheatre, long TimeStart, long TimeEnd)
        {
            string URL = "bookmyshowURLforyour city";  // "https://in.bookmyshow.com/hyderabad"; -- For Hyderabad
            int flag = 1;
            
            ChromeDriver driver = new ChromeDriver();
            
            driver.Navigate().GoToUrl(URL);

            Thread.Sleep(10000);

            driver.Manage().Window.Maximize();

            Thread.Sleep(5000);

            while (true)
            {
                if (flag == 1)
                {
                    driver.FindElement(By.XPath("//*[@id='wzrk-cancel']")).Click(); //Not Now
                    flag = 0;

                    driver.FindElement(By.XPath("//*[contains(text(),'" + MovieName + "')]")).Click(); //Find Movie Name and Click
                    Thread.Sleep(5000);

                    driver.FindElement(By.XPath("//*[contains(text(),'" + "Book Tickets" + "')]")).Click(); //Find Book Tickets and Click
                    Thread.Sleep(5000);

                    List<IWebElement> getdates = new List<IWebElement>(driver.FindElements(By.XPath("//*[@id='showDates']/li")));

                    foreach (var dates in getdates)
                    {
                        if ((dates.Text).Substring(0, 2).Equals((TimeStart.ToString()).Substring(6, 2)))
                        {
                            dates.Click();
                            Thread.Sleep(5000);
                            break;
                        }
                    }
                }
                for (int i = 0; i < PreferredTheatre.Count; i++)
                {
                    List<IWebElement> showtimes = new List<IWebElement>(driver.FindElements(By.XPath("//*[@data-name='" + PreferredTheatre[i] + "']/div[2]/div/a")));

                    if (showtimes.Count > 0)
                    {
                        foreach (var show in showtimes)
                        {
                            long datetime = Convert.ToInt64(show.GetAttribute("data-cut-off-date-time"));
                            if (datetime >= TimeStart && datetime <= TimeEnd)
                            {
                                show.Click();
                                driver.FindElement(By.XPath("//*[@id='btnPopupAccept']")).Click(); //Accept
                                Thread.Sleep(5000);

                                driver.FindElement(By.XPath("//*[@id='popQty']/li[" + 2 + "]")).Click(); //Select Seats
                                Thread.Sleep(5000);

                                driver.FindElement(By.XPath("//*[@id='proceed-Qty']")).Click(); //Select Seats
                                Thread.Sleep(5000);

                                sendMail(MovieName);
                                driver.Close();
                                driver.Quit();
                                break;
                            }
                        }
                    }
                    else
                    {
                        driver.Navigate().Refresh();
                    }
                }
                Thread.Sleep(1000000);
                continue;
            }
        }
        
        private static void sendMail(string MovieName)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                string _sender = "youremail@domain.com";
                string _password = "Password";
                string finalmail = "<html><body><h1>" + MovieName + "Tickets are available!</h1></body></html>";

                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(_sender, _password);
                client.EnableSsl = true;
                client.Credentials = credentials;

                var mail = new MailMessage();
                mail.To.Add("recipientemail@domain.com");
                mail.From = new MailAddress(_sender.Trim());
                mail.Subject = "Hurry! Book Tickets for " + MovieName;
                mail.Body = finalmail;
                mail.IsBodyHtml = true;
                client.Send(mail);
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.Write("Email sending failed!\nReason: " + e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
