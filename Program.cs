using System;
using System.Speech.Synthesis;
using System.Threading;
using System.Collections.Generic;
using Figgle;

class CyberSecurityChatbot
{
    static SpeechSynthesizer synth = new SpeechSynthesizer();

    // Memory system to store user information
    static Dictionary<string, string> userMemory = new Dictionary<string, string>();

    // Delegate for response selection
    delegate string ResponseSelector();

    // Dictionary to store keyword responses
    static Dictionary<string, ResponseSelector> keywordResponses = new Dictionary<string, ResponseSelector>();

    // Dictionary to store sentiment responses
    static Dictionary<string, string> sentimentResponses = new Dictionary<string, string>();

    static void Main()
    {
        InitializeResponses();
        InitializeSentimentResponses();
        DisplayAsciiArt();

        synth.Volume = 100;
        synth.Rate = 0;

        PrintWithTypingEffect(" Chatbot: Helloooooo! Welcome to the Cybersecurity Awareness Bot.", ConsoleColor.Yellow);
        synth.Speak("Hello! Welcome to the Cybersecurity Awareness Bot. I am here to help you stay safe online.");

        PrintDivider();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(" What's your name buddy? ");
        Console.ResetColor();
        string userName = Console.ReadLine();

        // Store the user's name in memory
        userMemory["name"] = userName;

        PrintWithTypingEffect($"\n Chatbot: Welcome, {userName}! I'm the Cybersecurity Awareness Bot.", ConsoleColor.Yellow);
        synth.Speak($"Welcome, {userName}! I'm the Cybersecurity Awareness Bot.");
        PrintWithTypingEffect("You can ask me about cybersecurity topics like phishing, password safety, and secure browsing. Don't worry I'll give you examples", ConsoleColor.Yellow);

        PrintDivider();
        // First check if we should reference a previous interest
        if (memory.ContainsKey("interest") && !input.Contains(memory["interest"]) && random.Next(3) == 0)
        {
            string interest = memory["interest"];

            // Find a matching keyword if possible
            foreach (var keyword in keywordResponses.Keys)
            {
                if (interest.Contains(keyword))
                {
                    Respond($"Since you're interested in {interest}, here's a tip: " +
                           keywordResponses[keyword][random.Next(keywordResponses[keyword].Count)], ConsoleColor.Cyan);
                    PrintDivider();
                    Respond("Would you like to ask another question? Type your question or type 'exit' to end the chat.", ConsoleColor.Green);
                    return;
                }
            }

            // Generic reference if no matching keyword
            Respond($"As someone interested in {interest}, it's important to stay updated on cybersecurity best practices.", ConsoleColor.Cyan);
            PrintDivider();
            Respond("Would you like to ask another question? Type your question or type 'exit' to end the chat.", ConsoleColor.Green);
            return;
        }
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nYou: ");
            Console.ResetColor();
            string input = Console.ReadLine().Trim().ToLower();

            if (string.IsNullOrEmpty(input))
            {
                PrintWithTypingEffect(" Please enter a valid question.", ConsoleColor.Red);
                synth.Speak("Please enter a valid question.");
                continue;
            }
            else if (input == "exit" || input == "quit")
            {
                PrintDivider();
                PrintWithTypingEffect($" Goodbye {userMemory["name"]}, See you soon!!!", ConsoleColor.Green);
                synth.Speak($"Goodbye {userMemory["name"]}, See you soon!");
                break;
            }
            else
            {
                RespondToUser(input);
            }
        }
    }

    static void InitializeResponses()
    {
        // Password keyword responses using delegate for random selection
        keywordResponses["password"] = () => {
            List<string> responses = new List<string>
            {
                "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                "A good password should be at least 12 characters long and include numbers, symbols, and both uppercase and lowercase letters.",
                "Consider using a password manager to create and store complex passwords securely.",
                "Remember to change your passwords regularly, especially for important accounts like banking and email."
            };
            return responses[new Random().Next(responses.Count)];
        };

        // Scam keyword responses
        keywordResponses["scam"] = () => {
            List<string> responses = new List<string>
            {
                "Be wary of offers that seem too good to be true. Scammers often use enticing offers to lure victims.",
                "Never send money or personal information to someone you've only met online without verifying their identity.",
                "Be suspicious of urgent requests for money or information, even if they appear to come from someone you know.",
                "Research unfamiliar companies or products before making purchases or investments online."
            };
            return responses[new Random().Next(responses.Count)];
        };

        // Privacy keyword responses
        keywordResponses["privacy"] = () => {
            List<string> responses = new List<string>
            {
                "Regularly review and update privacy settings on all your social media accounts and applications.",
                "Be careful about what personal information you share online. Once it's out there, it's hard to take back.",
                "Use private browsing modes and consider using a VPN when accessing sensitive information on public networks.",
                "Regularly clear your browser cookies and cache to prevent tracking of your online activities."
            };
            return responses[new Random().Next(responses.Count)];
        };

        // Phishing keyword responses
        keywordResponses["phishing"] = () => {
            List<string> responses = new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organizations.",
                "Always verify the sender's email address. Phishing emails often use addresses that look similar to legitimate ones but have slight differences.",
                "Don't click on suspicious links. Hover over links to see the actual URL before clicking.",
                "When in doubt about an email's legitimacy, contact the company directly using their official website or phone number."
            };
            return responses[new Random().Next(responses.Count)];
        };

        // Malware keyword responses
        keywordResponses["malware"] = () => {
            List<string> responses = new List<string>
            {
                "Keep your operating system and software updated to protect against known vulnerabilities.",
                "Use reliable antivirus software and keep it updated to detect and remove malicious programs.",
                "Be careful when downloading files or applications, especially from unfamiliar websites.",
                "Regularly scan your devices for malware and suspicious activities."
            };
            return responses[new Random().Next(responses.Count)];
        };

        // Topics keyword response
        keywordResponses["topics"] = () => {
            return "I can provide information on cybersecurity topics like passwords, privacy, scams, phishing, malware, and secure browsing. What would you like to learn about?";
        };
    }

    static void InitializeSentimentResponses()
    {
        // Responses for different sentiments
        sentimentResponses["worried"] = "It's completely understandable to feel worried about online security. Let me help ease your concerns with some practical advice: ";
        sentimentResponses["scared"] = "Many people feel scared about online threats. I'm here to help you feel more confident with these tips: ";
        sentimentResponses["confused"] = "Cybersecurity can be confusing! Let me break it down into simpler terms: ";
        sentimentResponses["frustrated"] = "I understand your frustration. Cybersecurity doesn't have to be overwhelming. Here's a straightforward approach: ";
        sentimentResponses["curious"] = "I'm glad you're curious about cybersecurity! Learning more is the first step to staying safe online. Here's what you should know: ";
        sentimentResponses["interested"] = "It's great that you're interested in this topic! Here's some valuable information: ";
    }

    static void DisplayAsciiArt()
    {
        string title = "Cybersecurity Awareness Bot";
        string asciiArt = FiggleFonts.Standard.Render(title);
        int width = title.Length + 100;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔" + new string('═', width) + "╗");
        Console.WriteLine($"║  {title}  ║");
        Console.WriteLine("╠" + new string('═', width) + "╣");
        Console.WriteLine(asciiArt);
        Console.WriteLine("╚" + new string('═', width) + "╝");
        Console.ResetColor();
    }

    static void PrintDivider()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n──────────────────────────────────────────────\n");
        Console.ResetColor();
    }

    static void PrintWithTypingEffect(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(20);
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    static void RespondToUser(string input)
    {
        // Check for sentiment words first
        string detectedSentiment = DetectSentiment(input);
        string sentimentPrefix = "";

        if (!string.IsNullOrEmpty(detectedSentiment))
        {
            sentimentPrefix = sentimentResponses[detectedSentiment];

            // Store the user's sentiment in memory
            userMemory["sentiment"] = detectedSentiment;
        }

        // Check for interest in specific topics to store in memory
        if (input.Contains("interested in"))
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    userMemory["interest"] = keyword;
                    PrintWithTypingEffect($" Chatbot: Great! I'll remember that you're interested in {keyword}. It's a crucial part of staying safe online.", ConsoleColor.Yellow);
                    synth.Speak($"Great! I'll remember that you're interested in {keyword}. It's a crucial part of staying safe online.");
                    return;
                }
            }
        }

        // Check if we should reference a previous interest
        if (userMemory.ContainsKey("interest") && !input.Contains(userMemory["interest"]) && new Random().Next(5) == 0)
        {
            PrintWithTypingEffect($" Chatbot: As someone interested in {userMemory["interest"]}, you might also want to know: ", ConsoleColor.Yellow);
            synth.Speak($"As someone interested in {userMemory["interest"]}, you might also want to know:");

            // Provide a response related to their interest
            string interestResponse = keywordResponses[userMemory["interest"]]();
            PrintWithTypingEffect($" {interestResponse}", ConsoleColor.Yellow);
            synth.Speak(interestResponse);

            PrintDivider();
            PrintWithTypingEffect(" Would you like to ask another question? Type your question or type 'exit' to end the chat.", ConsoleColor.Green);
            synth.Speak("Would you like to ask another question? Type your question or type exit to end the chat.");
            return;
        }

        // Handle standard hardcoded responses first
        if (input == "how are you?")
        {
            PrintWithTypingEffect(" Chatbot: I'm here to help you stay safe online!", ConsoleColor.Blue);
            synth.Speak("I'm here to help you stay safe online!");
        }
        else if (input == "what's your purpose?")
        {
            PrintWithTypingEffect(" Chatbot: My purpose is to provide cybersecurity awareness and help you stay safe online.", ConsoleColor.Yellow);
            synth.Speak("My purpose is to provide cybersecurity awareness and help you stay safe online.");
        }
        else if (input == "what can i ask you about?" || input == "help")
        {
            PrintWithTypingEffect(" Chatbot: You can ask me about phishing, password safety, secure browsing, and general online security.", ConsoleColor.Yellow);
            synth.Speak("You can ask me about phishing, password safety, secure browsing, and general online security.");
        }
        else if (input == "what is phishing?")
        {
            PrintWithTypingEffect(" Chatbot: Phishing is a cyber attack where scammers trick individuals into providing sensitive information like passwords or credit card details.", ConsoleColor.Yellow);
            PrintWithTypingEffect(" Example: A fake email pretending to be from your bank asking for your login credentials.", ConsoleColor.Yellow);
            synth.Speak("Phishing is a cyber attack where scammers trick individuals into providing sensitive information like passwords or credit card details. An example is a fake email pretending to be from your bank asking for your login credentials.");
        }
        else if (input == "what is password safety?")
        {
            PrintWithTypingEffect(" Chatbot: Well, Password safety involves creating strong passwords and keeping them secure from unauthorized access.", ConsoleColor.Yellow);
            PrintWithTypingEffect(" Example: Using a mix of uppercase, lowercase, numbers, and symbols in your password and enabling two-factor authentication.", ConsoleColor.Yellow);
            synth.Speak("Password safety involves creating strong passwords and keeping them secure from unauthorized access. An example is using a mix of uppercase, lowercase, numbers, and symbols in your password and enabling two-factor authentication.");
        }
        else if (input == "what is safe browsing?")
        {
            PrintWithTypingEffect("🛡 Chatbot: Safe browsing means using encrypted connections (HTTPS), avoiding suspicious links, and enabling privacy settings.", ConsoleColor.Yellow);
            PrintWithTypingEffect(" Example: Always check for 'HTTPS' in the URL before entering sensitive data.", ConsoleColor.Yellow);
            synth.Speak("Secure browsing means using encrypted connections, avoiding suspicious links, and enabling privacy settings. Always check for HTTPS in the URL before entering sensitive data.");
        }
        else
        {
            // Check for keywords in the input
            bool foundKeyword = false;
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    string response = keywordResponses[keyword]();

                    // Add sentiment prefix if detected
                    if (!string.IsNullOrEmpty(sentimentPrefix))
                    {
                        PrintWithTypingEffect($" Chatbot: {sentimentPrefix}", ConsoleColor.Yellow);
                        synth.Speak(sentimentPrefix);
                    }

                    PrintWithTypingEffect($" Chatbot: {response}", ConsoleColor.Yellow);
                    synth.Speak(response);
                    foundKeyword = true;
                    break;
                }
            }

            // If no keyword was found, provide a generic response
            if (!foundKeyword)
            {
                // Check if it's a follow-up question
                if (input.Contains("more") || input.Contains("details") || input.Contains("explain"))
                {
                    PrintWithTypingEffect(" Chatbot: I'd be happy to provide more information. Could you specify which cybersecurity topic you'd like to know more about?", ConsoleColor.Yellow);
                    synth.Speak("I'd be happy to provide more information. Could you specify which cybersecurity topic you'd like to know more about?");
                }
                else
                {
                    PrintWithTypingEffect(" Chatbot: I'm not sure how to answer that. Try asking about cybersecurity topics like passwords, privacy, scams, phishing, or malware.", ConsoleColor.Red);
                    synth.Speak("I'm not sure how to answer that. Try asking about cybersecurity topics like passwords, privacy, scams, phishing, or malware.");
                }
                
            }
        }

        PrintDivider();
        PrintWithTypingEffect(" Would you like to ask another question? Type your question or type 'exit' to end the chat.", ConsoleColor.Green);
        synth.Speak("Would you like to ask another question? Type your question or type exit to end the chat.");
    }

    static string DetectSentiment(string input)
    {
        // Check for sentiment keywords in user input
        foreach (var sentiment in sentimentResponses.Keys)
        {
            if (input.Contains(sentiment))
            {
                return sentiment;
            }
        }

        return null;
    }
}
