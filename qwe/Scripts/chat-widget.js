// Chat Widget JavaScript
(function() {
    let chatWindow = null;
    let chatMessages = null;
    let chatInput = null;
    let typingIndicator = null;

    // Initialize chat widget when DOM is ready
    document.addEventListener('DOMContentLoaded', function() {
        initializeChatWidget();
    });

    function initializeChatWidget() {
        // Create chat widget HTML
        const widgetHTML = `
            <div class="chat-widget">
                <button class="chat-button" onclick="toggleChat()">💬</button>
                <div class="chat-window" id="chat-window">
                    <div class="chat-header">
                        <h3>HHR CPA Assistant</h3>
                        <button class="chat-close" onclick="toggleChat()">×</button>
                    </div>
                    <div class="chat-messages" id="chat-messages">
                        <div class="chat-message assistant">
                            <div class="message-bubble">
                                Hello! I'm your HHR CPA virtual assistant. How can I help you today?
                            </div>
                        </div>
                    </div>
                    <div class="chat-message assistant" style="padding: 0 15px;">
                        <div class="typing-indicator" id="typing-indicator">
                            <div class="typing-dot"></div>
                            <div class="typing-dot"></div>
                            <div class="typing-dot"></div>
                        </div>
                    </div>
                    <div class="chat-input-area">
                        <input type="text" id="chat-input" class="chat-input" placeholder="Type your message..." />
                        <button class="chat-send-button" id="chat-send-button" onclick="sendMessage()">➤</button>
                    </div>
                </div>
            </div>
        `;

        // Add widget to body
        document.body.insertAdjacentHTML('beforeend', widgetHTML);

        // Get references to elements
        chatWindow = document.getElementById('chat-window');
        chatMessages = document.getElementById('chat-messages');
        chatInput = document.getElementById('chat-input');
        typingIndicator = document.getElementById('typing-indicator');

        // Add enter key listener
        chatInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                sendMessage();
            }
        });
    }

    // Make functions global so they can be called from onclick
    window.toggleChat = function() {
        if (chatWindow) {
            chatWindow.classList.toggle('active');
            if (chatWindow.classList.contains('active')) {
                chatInput.focus();
            }
        }
    };

    window.sendMessage = async function() {
        const message = chatInput.value.trim();
        if (!message) return;

        // Disable input while processing
        chatInput.disabled = true;
        document.getElementById('chat-send-button').disabled = true;

        // Add user message to chat
        addMessageToChat('user', message);
        chatInput.value = '';

        // Show typing indicator
        typingIndicator.classList.add('active');
        scrollToBottom();

        try {
            // Send message to API
            const response = await fetch('/api/chat', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ Message: message })
            });

            if (!response.ok) {
                throw new Error('Failed to get response');
            }

            const data = await response.json();

            // Hide typing indicator
            typingIndicator.classList.remove('active');

            // Add assistant response to chat
            addMessageToChat('assistant', data.Response);
        } catch (error) {
            console.error('Error sending message:', error);
            typingIndicator.classList.remove('active');
            addMessageToChat('assistant', 'Sorry, I encountered an error. Please try again or contact us directly at contact@hhrcpa.us');
        } finally {
            // Re-enable input
            chatInput.disabled = false;
            document.getElementById('chat-send-button').disabled = false;
            chatInput.focus();
        }
    };

    function addMessageToChat(role, content) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `chat-message ${role}`;

        const now = new Date();
        const timeString = now.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' });

        messageDiv.innerHTML = `
            <div class="message-bubble">${escapeHtml(content)}</div>
            <div class="message-time">${timeString}</div>
        `;

        chatMessages.appendChild(messageDiv);
        scrollToBottom();
    }

    function scrollToBottom() {
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }

    function escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }
})();
