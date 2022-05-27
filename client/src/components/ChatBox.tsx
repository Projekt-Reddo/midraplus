import moment from "moment";
import * as React from "react";
import Icon from "./Icon";
import { useDispatch, useSelector } from "react-redux";
import { SEND_MESSAGE } from "store/actions";

interface ChatBoxProps {}

const ChatBox: React.FC<ChatBoxProps> = () => {
    const dispatch = useDispatch();
    const user = useSelector((state: any) => state.user);

    const handleSubmit = (message: string) => {
        dispatch({
            type: SEND_MESSAGE,
            payload: {
                user: user.user,
                message,
            },
        });
    };

    return (
        <div
            className="rounded fixed right-10 chat-box__container"
            style={{
                bottom: "5.6rem",
            }}
        >
            <div className="w-full h-full relative">
                <ChatBoxBody user={user.user} />
                <ChatBoxInput handleSubmit={handleSubmit} />
            </div>
        </div>
    );
};

export default ChatBox;

interface ChatBoxBodyProps {
    user: any;
}

const ChatBoxBody: React.FC<ChatBoxBodyProps> = ({ user }) => {
    const chat = useSelector((state: any) => state.chat);

    const endChat = React.useRef<null | HTMLDivElement>(null);

    React.useEffect(() => {
        if (endChat.current) {
            endChat.current.scrollIntoView({ behavior: "smooth" });
        }
    }, [chat]);

    return (
        <div className="chat-box__body">
            {chat.map((item: any, index: number, array: any[]) => (
                <ChatBoxMessage
                    key={index}
                    item={item}
                    previousItem={array[index - 1]}
                    user={user}
                />
            ))}
            <div ref={endChat}></div>
        </div>
    );
};

interface ChatBoxMessageProps {
    item: any;
    previousItem: any;
    user: any;
}

const ChatBoxMessage: React.FC<ChatBoxMessageProps> = ({
    item,
    previousItem,
    user,
}) => {
    return (
        <div
            className={`w-full px-3 py-2 flex ${
                user.id === item.user.id ? "flex-row-reverse" : "flex-row"
            } flex-wrap`}
        >
            <div className="w-1/5 text-center">
                {previousItem?.user?.id === item.user?.id ? (
                    <></>
                ) : (
                    <img
                        src={item.user.avatar}
                        alt="avatar"
                        className="inline-block h-10 w-10 rounded-full ring-2 ring-white object-cover"
                    />
                )}
            </div>
            <div className="w-4/5 px-1">
                <div
                    className="rounded text-sm p-2"
                    style={{
                        backgroundColor:
                            user.id === item.user.id ? "#007aff" : "#fff",
                        color: user.id === item.user.id ? "#fff" : "#000",
                    }}
                >
                    {previousItem?.user?.id === item.user?.id ? (
                        <></>
                    ) : (
                        <>
                            <span className="inline-block font-bold">
                                {item.user.name}
                            </span>
                            <br />
                        </>
                    )}
                    <span className="inline-block">{item.message}</span>
                    <br />
                    <span className="inline-block w-full text-xs text-right mt-1 opacity-50">
                        {moment(item.timestamp).format("HH:mm")}
                    </span>
                </div>
            </div>
        </div>
    );
};

interface ChatBoxInputProps {
    handleSubmit: (message: string) => void;
}

const ChatBoxInput: React.FC<ChatBoxInputProps> = ({ handleSubmit }) => {
    const [message, setMessage] = React.useState("");

    const onSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (message.trim()) {
            handleSubmit(message);
            setMessage("");
        }
    };

    return (
        <form
            className="chat-box__chat-bar rounded absolute pl-3 pr-2 flex items-center justify-between"
            onSubmit={onSubmit}
        >
            <input
                className="chat-box__input"
                placeholder="Start typing......"
                value={message}
                onChange={(e) => setMessage(e.target.value)}
            />
            <button type="submit">
                <Icon icon="paper-plane" size="lg" />
            </button>
        </form>
    );
};
