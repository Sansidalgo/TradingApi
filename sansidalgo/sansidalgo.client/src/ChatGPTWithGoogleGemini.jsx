// Import necessary libraries and components
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
            return null;
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

    const handleSendMessage = async (e) => {
        if (e && e.preventDefault) {
            e.preventDefault();
        }

        if (inputValue.trim() === '') return;

        const currentInputValue = inputValue;

        setMessages([...messages, { text: currentInputValue, sender: 'user' }]);
        setInputValue('');

        const first50Chars = currentInputValue.slice(0, 50);
        addToHistory(first50Chars);

        const modelType = fileInputRef.current.files.length > 0 ? "gemini-pro-vision" : "gemini-pro";
        const model = genAI.getGenerativeModel({ model: modelType });

        const imageParts = await Promise.all(
            [...fileInputRef.current.files].map(fileToGenerativePart)
        );

        try {
            setIsLoading(true);

            const result = await model.generateContent([currentInputValue, ...imageParts.filter(part => part !== null)]);
            const response = await result.response;
            const text = await response.text();

            await addToContent(currentInputValue, text);
        } catch (error) {
            console.error(error);
            await addToContent(currentInputValue, error);
        } finally {
            setIsLoading(false);
        }
        document.getElementById('btnSendMessage').focus();
    };

    const addToContent = async (yourText, text) => {
        const contentDivStart = document.getElementById('messsageContentstart');
        if (contentDivStart) {
            contentDivStart.remove();
        }
       
        const contentDiv = document.getElementById('messsageContent');
        if (contentDiv) {
            const contentItem = document.createElement('p');
            contentItem.textContent = text;
            contentDiv.insertBefore(contentItem, contentDiv.firstChild);

            const tradeSynergy = document.createElement('h4');
            tradeSynergy.textContent = "ChatX";
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
        const historyDiv = document.querySelector('.sidebar');
        if (historyDiv) {
            const historyItem = document.createElement('a');
            historyItem.textContent = text;
            if (historyDiv.childElementCount > 5) {
                historyDiv.removeChild(historyDiv.lastChild);
            }
            historyDiv.insertBefore(historyItem, historyDiv.firstChild.nextSibling);
        }
    };

    const toggleMenu = () => {
        const sidebar = document.querySelector('.sidebar');
        if (sidebar) {
            sidebar.style.display = sidebar.style.display === 'none' ? 'block' : 'none';
        }
    };

    return (
        <section >
            <div className="sidebar" >
                <a className="active" href="#home">New Chat</a>

            </div>
            <div className="content">

                <div id="messsageContentstart"  className="messagecontainer-start">
                    <h2>Welcome to our ChatX</h2>
                    <p>Engage in insightful conversations with our advanced Chat Generative AI...</p>
                    <div className="detail-box">
                        <div>
                            <p>Discover the power of natural language processing and elevate your interactions...</p>
                        </div>
                    </div>
                </div>
                <div id="messsageContent" className="messagecontainer">
                   
                </div>
               
                <div className="chat-input-container">
                    <label htmlFor="fileInput" style={{ display: 'inline-block', marginRight: '8px', verticalAlign: 'middle' }}>
                        <i className="fas fa-image"></i>
                        <input
                            type="file"
                            ref={fileInputRef}
                            id="fileInput"
                            style={{ display: 'none' }}
                        />
                    </label>

                    <input
                        type="text"
                        placeholder="Type your message..."
                        value={inputValue}
                        onChange={(e) => setInputValue(e.target.value)}
                        onKeyPress={(e) => { e && e.key === 'Enter' && handleSendMessage(e); }}
                        style={{ flex: '1', padding: '8px', marginRight: '8px', border: '1px solid green', borderRadius: '5px', verticalAlign: 'middle' }}
                    />

                    <a href="#" id="btnSendMessage" onClick={(e) => { e && handleSendMessage(e); }} style={{ padding: '8px', width: '50px', borderRadius: '5px', fontSize: '16px', verticalAlign: 'middle' }}>
                        <i className="fas fa-arrow-right"></i>
                    </a>
                   
                    <div>
                        {isLoading && (<>
                            <p style={{ color: 'red' }}>....content is loading, please wait....</p>
                        </>)}
                    </div>

                </div>

                
            </div>
        </section>
    );
}

export default ChatGPTWithGoogleGemini;
