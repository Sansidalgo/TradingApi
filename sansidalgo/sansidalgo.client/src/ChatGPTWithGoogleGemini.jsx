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

        setMessages([...messages, { text: inputValue, sender: 'user' }]);
        setInputValue('');
        // Add the first 50 characters of the message to the history
        const first50Chars = inputValue.slice(0, 50);
        addToHistory(first50Chars);
        // Use Google Gemini to get chatbot response
        const modelType = fileInputRef.current.files.length > 0 ? "gemini-pro-vision" : "gemini-pro";
        const model = genAI.getGenerativeModel({ model: modelType });
        const prompt = inputValue;

        const imageParts = await Promise.all(
            [...fileInputRef.current.files].map(fileToGenerativePart)
        );

        try {
            setIsLoading(true);
            const result = await model.generateContent([prompt, ...imageParts.filter(part => part !== null)]);
            const response = await result.response;
            const text = await response.text();

            setMessages([...messages, { text, sender: 'chatbot' }]);
        } catch (error) {
            console.error(error);
            setMessages([...messages, { error, sender: 'chatbot' }]);
            // Handle error, display a message, etc.
        }
        finally {
            setIsLoading(false); // Set loading state to false
        }
    };
    const addToHistory = (text) => {
        // Add the first 50 characters of the message to the history
        const historyDiv = document.querySelector('.history-container');
        if (historyDiv) {
            const historyItem = document.createElement('div');
            historyItem.textContent = text;
            historyDiv.appendChild(historyItem);
        }
    };
    return (
        <div className="container-fluid" style={{ display: 'flex', marginTop: '10px' }}>
            {/* History Column */}
            <div className="col-lg-2 col-md-2" style={{ flex: '2' }}>
                <div className="history-container">
                    <p> History</p>
                </div>
            </div>
            {/* Chat Column */}
            <div className="col-lg-10 col-md-10" style={{ flex: '8' }}>
                <div className="boxTypeDiv chat-panel">
                    <div className="chat-messages" style={{ maxHeight: '20vh', overflowY: 'auto' }}>
                        {isLoading ? (
                            <div className="loading-indicator" >
                                <p>....loading....</p>
                            </div>
                        ) : (
                            messages.length > 0 ? (
                                messages.map((message, index) => (
                                    <div key={index} className="default-content" >
                                        {message.text}
                                    </div>
                                ))
                            ) : (
                                        <div className="default-content" >
                                    <h2>Welcome to Trade Synergies GPT</h2>
                                    <p>Tell me what is on your mind, or pick a suggestion.</p>
                                    <div className="detail-box">
                                        <div>
                                            <p>Elevate Your Trading with Our Premier Services</p>
                                        </div>
                                    </div>
                                </div>
                            )
                        )}
                    </div>
                    <div className="input-container" style={{
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
            </div>
        </div>
    );
}
export default ChatGPTWithGoogleGemini;
