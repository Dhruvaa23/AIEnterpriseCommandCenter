const aiButton = document.getElementById("aiButton");

const chatWindow = document.getElementById("chatWindow");

const closeChat = document.getElementById("closeChat");

const chatBox = document.getElementById("chatMessages");

window.addEventListener("load", () => {

    chatBox.innerHTML = `

<div class="ai-message">

    <div class="avatar">🤖</div>

    <div class="bubble">

        <strong>Welcome 👋</strong><br><br>

        I am your AI Enterprise Assistant.

        <br><br>

        I can help today?:

        <div class="quick-actions">

    <button class="quick-chip" onclick="sendQuickMessage('Employee count')">
        👥 Employees
    </button>

    <button class="quick-chip" onclick="sendQuickMessage('Available assets')">
        💻 Assets
    </button>

    <button class="quick-chip" onclick="sendQuickMessage('Open tickets')">
        🎫 Tickets
    </button>

    <button class="quick-chip" onclick="sendQuickMessage('Dashboard summary')">
        📊 Dashboard
    </button>

</div>

    </div>

</div>

`;

});

aiButton.onclick = () => {

    chatWindow.style.display = "flex";

}

closeChat.onclick = () => {

    chatWindow.style.display = "none";

}

document.getElementById("clearChat")
    .addEventListener("click", async () => {

        await fetch("/AI/ClearChat", {

            method: "POST"

        });

        chatBox.innerHTML = "";

    });

const textbox = document.getElementById("message");
const sendBtn = document.getElementById("sendBtn");

sendBtn.addEventListener("click", sendMessage);

textbox.addEventListener("keypress", function (e) {
    if (e.key === "Enter") {
        sendMessage();
    }
});

function addMessage(sender, message) {

    const isUser = sender === "You";

    chatBox.innerHTML += `
        <div class="${isUser ? "user-message" : "ai-message"}">

            ${!isUser ? `<div class="avatar"><i class="bi bi-robot"></i></div>` : ""}

            <div class="bubble">
                ${message}
            </div>

            ${isUser ? `<div class="avatar"><i class="bi bi-person-fill"></i></div>` : ""}

        </div>
    `;

    chatBox.scrollTop = chatBox.scrollHeight;
}

window.sendQuickMessage = function (message) {

    const textbox = document.getElementById("message");

    if (!textbox) {
        console.error("Textbox not found");
        return;
    }

    textbox.value = message;

    sendMessage();
}

async function sendMessage() {

    const textbox = document.getElementById("message");
    const sendBtn = document.getElementById("sendBtn");

    let msg = textbox.value.trim();

    if (msg === "")
        return;

    const chips = document.querySelector(".quick-actions");

    if (chips) {
        chips.style.display = "none";
    }

    addMessage("You", msg);

    textbox.value = "";

    sendBtn.disabled = true;

    chatBox.innerHTML += `
<div id="typing" class="ai-message">
    <div class="avatar"><i class="bi bi-robot"></i></div>
    <div class="bubble">
        <span class="typing-dots">
            <span></span>
            <span></span>
            <span></span>
        </span>
    </div>
</div>
`;

    chatBox.scrollTop = chatBox.scrollHeight;

    try {

        const response = await fetch("/AI/Ask", {

            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                prompt: msg
            })

        });

        const data = await response.json();

        document.getElementById("typing").remove();

        addMessage("AI", data.response);

    }
    catch (err) {

        document.getElementById("typing").remove();

        addMessage("AI", "❌ Unable to connect to AI service.");

        console.error(err);

    }

    sendBtn.disabled = false;

    textbox.focus();

}