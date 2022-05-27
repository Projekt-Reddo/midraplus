import * as React from "react";
import Icon from "./Icon";
import "styles/Chat.css";
import ChatBox from "./ChatBox";

interface ChatProps {}

const Chat: React.FC<ChatProps> = () => {
    const [showChatBox, setShowChatBox] = React.useState(false);

    return (
        <>
            <ChatButton onClick={() => setShowChatBox(!showChatBox)} />
            {showChatBox && <ChatBox />}
        </>
    );
};

export default Chat;

interface ChatButtonProps {
    onClick: () => void;
}

const ChatButton: React.FC<ChatButtonProps> = ({ onClick }) => {
    return (
        <div
            className="rounded-full fixed right-10 bottom-7 flex items-center justify-center chat-btn"
            onClick={onClick}
        >
            <Icon icon="comment" fontSize="1.25rem" />
        </div>
    );
};
