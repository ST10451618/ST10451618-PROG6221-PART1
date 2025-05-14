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

            // Reference interest if remembered
            if (userMemory.ContainsKey("interest") && !input.Contains(userMemory["interest"]) && new Random().Next(5) == 0)
            {
                string interest = userMemory["interest"];
                PrintWithTypingEffect($" Chatbot: Since you're interested in {interest}, here's a tip:", ConsoleColor.Cyan);
                synth.Speak($"Since you're interested in {interest}, here's a tip:");
                string interestResponse = keywordResponses[interest]();
                PrintWithTypingEffect($" {interestResponse}", ConsoleColor.Cyan);
                synth.Speak(interestResponse);
                PrintDivider();
                continue;
            }

            RespondToUser(input);
        }
    }

    static void InitializeResponses()
    {
        keywordResponses["password"] = () => {
            List<string> responses = new List<string>
            {
                "Use strong, unique passwords for each account. Avoid using personal info.",
                "Make passwords long—12+ characters—with numbers, symbols, and mixed case.",
                "A password manager can help you store and generate secure passwords.",
                "Change important passwords regularly, especially for email and banking."
            };
            return responses[new Random().Next(responses.Count)];
        };

        keywordResponses["scam"] = () => {
            List<string> responses = new List<string>
            {
                "Be wary of offers that seem too good to be true.",
                "Don't send personal info to strangers online.",
                "Scammers often use urgency to pressure victims—think before you act.",
                "Always verify unfamiliar contacts or companies."
            };
            return responses[new Random().Next(responses.Count)];
        };

        keywordResponses["privacy"] = () => {
            List<string> responses = new List<string>
            {
                "Review and update your privacy settings regularly.",
                "Limit what you share publicly on social media.",
                "Use private browsing and VPNs when needed.",
                "Clear cookies and cache often to avoid tracking."
            };
            return responses[new Random().Next(responses.Count)];
        };

        keywordResponses["phishing"] = () => {
            List<string> responses = new List<string>
            {
                "Phishing emails try to trick you into revealing personal info.",
                "Check sender emails carefully—they often look legit but are fake.",
                "Don't click suspicious links. Hover first to see the real address.",
                "When unsure, contact companies directly through official sites."
            };
            return responses[new Random().Next(responses.Count)];
        };

        keywordResponses["malware"] = () => {
            List<string> responses = new List<string>
            {
                "Update your OS and software to patch vulnerabilities.",
                "Use trusted antivirus software and keep it updated.",
                "Avoid downloading files from unknown sources.",
                "Scan devices regularly for threats."
            };
            return responses[new Random().Next(responses.Count)];
        };

        keywordResponses["topics"] = () => {
            return "I can help with topics like passwords, scams, phishing, privacy, malware, and safe browsing. Ask me anything!";
        };
    }

    static void InitializeSentimentResponses()
    {
        sentimentResponses["worried"] = "It's okay to feel worried. Let's take a step to secure your online life.";
        sentimentResponses["scared"] = "Don't worry! I’m here to help you feel confident about online safety.";
        sentimentResponses["confused"] = "Cybersecurity can be tricky! Here's something to help you understand.";
        sentimentResponses["frustrated"] = "I understand—tech stuff can be annoying. Let's simplify it.";
        sentimentResponses["curious"] = "Curiosity is the first step! Let's explore cybersecurity together.";
        sentimentResponses["interested"] = "Great to hear you're interested! Here's a helpful tip:";
    }

    static string DetectSentiment(string input)
    {
        foreach (var sentiment in sentimentResponses.Keys)
        {
            if (input.Contains(sentiment))
                return sentiment;
        }
        return null;
    }

    static void RespondToUser(string input)
    {
        string detectedSentiment = DetectSentiment(input);
        string sentimentPrefix = "";

        if (!string.IsNullOrEmpty(detectedSentiment))
        {
            sentimentPrefix = sentimentResponses[detectedSentiment];
            userMemory["sentiment"] = detectedSentiment;
        }

        if (input.Contains("interested in"))
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    userMemory["interest"] = keyword;
                    PrintWithTypingEffect($" Chatbot: Got it! You're interested in {keyword}. I'll remember that.", ConsoleColor.Yellow);
                    synth.Speak($"Got it! You're interested in {keyword}. I'll remember that.");
                    return;
                }
            }
        }

        
        if (input == "how are you?")
        {
            PrintWithTypingEffect(" Chatbot: I'm fantastic, I'm a Robot LOL and I hope you are great too!", ConsoleColor.Green);
            synth.Speak(" I'm fantastic, I'm a Robot LOL and I hope you are great too!");
        }
        else if (input == "what's your purpose?")
        {
            PrintWithTypingEffect(" Chatbot: My purpose is to teach and guide you on cybersecurity awareness.", ConsoleColor.Yellow);
            synth.Speak("My purpose is to teach and guide you on cybersecurity awareness.");
        }
        else if (input == "what can i ask you about?" || input == "help")
        {
            PrintWithTypingEffect(" Chatbot: You can ask about phishing, passwords, privacy, scams, and safe browsing.", ConsoleColor.Yellow);
            synth.Speak("You can ask about phishing, passwords, privacy, scams, and safe browsing.");
        }
        else
        {
            bool found = false;
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    string response = keywordResponses[keyword]();

                    if (!string.IsNullOrEmpty(sentimentPrefix))
                    {
                        PrintWithTypingEffect($" Chatbot: {sentimentPrefix}", ConsoleColor.Yellow);
                        synth.Speak(sentimentPrefix);
                    }

                    PrintWithTypingEffect($" Chatbot: {response}", ConsoleColor.Yellow);
                    synth.Speak(response);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                if (input.Contains("more") || input.Contains("details") || input.Contains("explain"))
                {
                    PrintWithTypingEffect(" Chatbot: Can you clarify which topic you'd like more info on?", ConsoleColor.Yellow);
                    synth.Speak("Can you clarify which topic you'd like more info on?");
                }
                else
                {
                    PrintWithTypingEffect(" Chatbot: I didn’t quite get that. Ask me about phishing, malware, scams, or password safety.", ConsoleColor.Red);
                    synth.Speak("I didn’t quite get that. Ask me about phishing, malware, scams, or password safety.");
                }
            }
        }

        PrintDivider();
        PrintWithTypingEffect(" Would you like to ask another question? Type your question or type 'exit' to end the chat.", ConsoleColor.Green);
        synth.Speak("Would you like to ask another question? Type your question or type exit to end the chat.");
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
}
