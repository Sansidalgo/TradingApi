import React, { useState, useRef } from 'react';
import { GoogleGenerativeAI } from "@google/generative-ai";
import './chatgptgemini.css';
const ChatGPTWithGoogleGemini = () => {

    const [messages, setMessages] = useState([]);
    const [inputValue, setInputValue] = useState('');
    const fileInputRef = useRef();
    const [isLoading, setIsLoading] = useState(false);

    const API_KEY = 'AIzaSyBtsjOcmjkWDfqy9RIGn6gfpCZs7rgor6s'; // Replace with your actual API key
    const genAI = new GoogleGenerativeAI(API_KEY);

    const fileToGenerativePart = async (file) => {
        if (!file) {
            return null; // Return null if no file is provided
        }

        const base64EncodedDataPromise = new Promise((resolve) => {
            const reader = new FileReader();
            reader.onloadend = () => resolve(reader.result.split(',')[1]);
            reader.readAsDataURL(file);
        });

        return {
            inlineData: { data: await base64EncodedDataPromise, mimeType: file.type },
        };
    };

    const handleSendMessage = async () => {
        if (inputValue.trim() === '') return;

        // Capture the current inputValue before making the asynchronous call
        const currentInputValue = inputValue;

        setMessages([...messages, { text: currentInputValue, sender: 'user' }]);
        setInputValue('');

        // Add the first 50 characters of the message to the history
        const first50Chars = currentInputValue.slice(0, 50);
        addToHistory(first50Chars);

        // Use Google Gemini to get chatbot response
        const modelType = fileInputRef.current.files.length > 0 ? "gemini-pro-vision" : "gemini-pro";
        const model = genAI.getGenerativeModel({ model: modelType });

        const imageParts = await Promise.all(
            [...fileInputRef.current.files].map(fileToGenerativePart)
        );

        try {
            setIsLoading(true);

            // Use the captured currentInputValue in the following line
            const result = await model.generateContent([currentInputValue, ...imageParts.filter(part => part !== null)]);
            const response = await result.response;
            const text = await response.text();
            
            await addToContent(currentInputValue,text);
           /* setMessages([...messages, { text, sender: 'chatbot' }]);*/
        } catch (error) {
            console.error(error);
            await addToContent(currentInputValue, error);
            // Handle error, display a message, etc.
        } finally {
            setIsLoading(false); // Set loading state to false
        }
    };
    const addToContent = async (yourText, text) => {
        const contentDiv = document.getElementById('messsageContent');
        if (contentDiv) {


            const contentItem = document.createElement('p');
            contentItem.textContent = text;
            contentDiv.insertBefore(contentItem, contentDiv.firstChild);

            const tradeSynergy = document.createElement('h4');
            tradeSynergy.textContent = "Trade Synergy";
            contentDiv.insertBefore(tradeSynergy, contentItem);
        }
        if (contentDiv) {
           

            const yourItem = document.createElement('p');
            yourItem.textContent = yourText;
            contentDiv.insertBefore(yourItem, contentDiv.firstChild);

            const your = document.createElement('h4');
            your.textContent = "You";
            contentDiv.insertBefore(your, yourItem);
        }

        
    };

    const addToHistory = (text) => {
        // Add the first 50 characters of the message to the history
        const historyDiv = document.querySelector('.sidebar');
        if (historyDiv) {
            const historyItem = document.createElement('a');
            historyItem.textContent = text;
            if (historyDiv.childElementCount > 5) {
                // Remove the first child
                historyDiv.removeChild(historyDiv.lastChild);
            }
            historyDiv.insertBefore(historyItem, historyDiv.firstChild.nextSibling);
            
        }
    };
    return (
        <section>
            <div class="sidebar">
                <a class="active" href="#home">New Chat</a>
               
            </div>

            <div class="content">
                <div  style={{maxHeight:'100vh', overflowY: 'auto' }}>
                    {isLoading ? (
                        <div className="loading-indicator" style={{ minHeight: '100vh' }}>
                            <p>....loading....</p>
                        </div>
                    ):(<div></div>) 
                    }
                    <div id="messsageContent" style={{ minHeight: '100vh' }}>
                        <h2>Welcome to Trade Synergies GPT</h2>
                        <p>Tell me what is on your mind, or pick a suggestion.</p>
                        <div className="detail-box">
                            <div>
                                <p>Elevate Your Trading with Our Premier Services</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div  style={{
                    display: 'flex',
                    alignItems: 'center',
                    position: 'sticky',
                    bottom: '0',
                    width: '100%',
                    background: '#fff', // Add a background color if needed
                    padding: '10px', // Add padding as needed
                    boxShadow: '0px -1px 5px rgba(0, 0, 0, 0.1)' // Optional: Add a shadow for separation
                }}>
                    {/* Upload button with image icon */}
                    <label htmlFor="fileInput" style={{ display: 'inline-block', marginRight: '8px', verticalAlign: 'middle' }}>
                        <i className="fas fa-image"></i>
                        <input
                            type="file"
                            ref={fileInputRef}
                            id="fileInput"
                            style={{ display: 'none' }}
                        />
                    </label>

                    {/* Input field */}
                    <input
                        type="text"
                        placeholder="Type your message..."
                        value={inputValue}
                        onChange={(e) => setInputValue(e.target.value)}
                        onKeyPress={(e) => e.key === 'Enter' && handleSendMessage()}
                        style={{ flex: '1', padding: '8px', marginRight: '8px', border: '1px solid #ccc', borderRadius: '5px', verticalAlign: 'middle' }}
                    />

                    {/* Send button with right arrow icon */}
                    <a onClick={handleSendMessage} style={{ padding: '8px', width: '50px', borderRadius: '5px', fontSize: '16px', verticalAlign: 'middle' }}>
                        <i className="fas fa-arrow-right"></i>
                    </a>
                </div>
            </div>
        </section>
    );
}
export default ChatGPTWithGoogleGemini;
