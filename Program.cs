// Cybersecurity Awareness Chatbot - Part 2
using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Threading;
using Figgle;

class CyberSecurityChatbot
{
    static SpeechSynthesizer synth = new SpeechSynthesizer();
    static Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>();
    static Dictionary<string, string> memory = new Dictionary<string, string>();
    static Random random = new Random();

    static void Main()
    {
        InitialiseResponses();
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
        memory["name"] = userName;

        PrintWithTypingEffect($"\n Chatbot: Welcome, {userName}! I'm the Cybersecurity Awareness Bot.", ConsoleColor.Yellow);
        synth.Speak($"Welcome, {userName}! I'm the Cybersecurity Awareness Bot.");

        PrintWithTypingEffect("You can ask me about cybersecurity topics like phishing, password safety, and secure browsing.", ConsoleColor.Yellow);
        PrintDivider();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nYou: ");
            Console.ResetColor();
            string input = Console.ReadLine().Trim().ToLower();

            if (string.IsNullOrEmpty(input))
            {
                Respond("Please enter a valid question.", ConsoleColor.Red);
                continue;
            }
            else if (input == "exit" || input == "quit")
            {
                PrintDivider();
                Respond($"Goodbye {userName}, See you soon!!!", ConsoleColor.Green);
                break;
            }
            else
            {
                ProcessInput(input);
            }
        }
    }

    static void InitialiseResponses()
    {
        keywordResponses["password"] = new List<string>
        {
            "Use a mix of uppercase, lowercase, numbers, and symbols.",
            "Avoid using personal info in your password.",
            "Enable two-factor authentication for extra protection."
        };

        keywordResponses["phishing"] = new List<string>
        {
            "Be cautious of emails asking for login details.",
            "Don’t click suspicious links—hover to preview URLs.",
            "Verify the sender before responding to any message."
        };

        keywordResponses["privacy"] = new List<string>
        {
            "Adjust your social media privacy settings.",
            "Use encrypted messaging apps.",
            "Regularly review permissions of installed apps."
        };
    }

    static void ProcessInput(string input)
    {
        if (input.Contains("how are you"))
        {
            Respond("I'm great to help you stay safe online!", ConsoleColor.Blue);
        }
        
        else if (input.Contains("your purpose"))
        {
            Respond("My purpose is to provide cybersecurity awareness and help you stay safe online.", ConsoleColor.Yellow);
        }
        else if (input.Contains("what can i ask"))
        {
            Respond("You can ask me about phishing, password safety, secure browsing, and general online security.", ConsoleColor.Yellow);
        }
        else if (DetectSentiment(input))
        {
            return;
        }
        else if (CheckForMemoryTrigger(input))
        {
            return;
        }
        else
        {
            bool found = false;
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    found = true;
                    string tip = keywordResponses[keyword][random.Next(keywordResponses[keyword].Count)];
                    Respond($"Tip about {keyword}: {tip}", ConsoleColor.Yellow);
                    break;
                }
            }

            if (!found)
            {
                Respond("I'm not sure I understand. Can you try rephrasing?", ConsoleColor.Red);
            }
        }

        Respond("Would you like to ask another question? Type your question or type 'exit' to end the chat.", ConsoleColor.Green);
    }

    static bool DetectSentiment(string input)
    {
        if (input.Contains("worried"))
        {
            Respond("It's completely understandable to feel that way. Let me share some tips to help you stay safe.", ConsoleColor.Magenta);
            return true;
        }
        else if (input.Contains("frustrated"))
        {
            Respond("I'm here to help. Cybersecurity can be tricky, but you're doing great just by asking questions!", ConsoleColor.Magenta);
            return true;
        }
        else if (input.Contains("curious"))
        {
            Respond("Curiosity is great! Let’s explore cybersecurity topics together.", ConsoleColor.Magenta);
            return true;
        }
        return false;
    }

    static bool CheckForMemoryTrigger(string input)
    {
        if (input.Contains("interested in"))
        {
            string topic = input.Substring(input.IndexOf("interested in") + 13).Trim();
            memory["interest"] = topic;
            Respond($"Great! I'll remember that you're interested in {topic}. It's a crucial part of staying safe online.", ConsoleColor.Cyan);
            return true;
        }
        else if (memory.ContainsKey("interest") && input.Contains("privacy") || input.Contains("remind me"))
        {
            string interest = memory["interest"];
            Respond($"As someone interested in {interest}, you might want to review the security settings on your accounts.", ConsoleColor.Cyan);
            return true;
        }
        return false;
    }

    static void Respond(string message, ConsoleColor color)
    {
        PrintWithTypingEffect(" Chatbot: " + message, color);
        synth.Speak(message);
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
