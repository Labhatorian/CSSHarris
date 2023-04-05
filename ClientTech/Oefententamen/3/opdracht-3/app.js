class ChatBox {


    toggleChat() {

//student uitwerking
    let box = document.getElementById('chatbox');
    let send = document.getElementById('toonKnop');
    box.classList.remove("chatbox--closed");
    send.classList.add("toonKnop--closed");

    let sendbutton = document.getElementById('verstuurKnop');
     sendbutton.addEventListener('click', () => {
                    this.sendMessage();
                });
    }

    async sendMessage() {

//student uitwerking
let messagebox = document.getElementById('chatbox__message');

const response = await fetch("http://127.0.0.1:3000/chat", {
    method: 'POST',
    headers: {
        "Content-Type": "application/json",
      },
    body: JSON.stringify({message: messagebox.value})
  });

        console.log(await response.json());
        messagebox.value = "";
        let box = document.getElementById('chatbox');
        let send = document.getElementById('toonKnop');
        box.classList.add("chatbox--closed");
        send.classList.remove("toonKnop--closed");
    }


}

// document ready...
$(() => {

    // let chatBox = new ChatBox();
    // let button = document.getElementById('toonKnop');

//student uitwerking
     let chatBox = new ChatBox();
     let button = document.getElementById('toonKnop');
     button.addEventListener('click', () => {
                    chatBox.toggleChat();
                });  
});




