import { useState } from 'react';

interface NotifyMessage {
    icon: string,
    iconColor?: string,
    title: string,
    message?: string,
}

export const useNotification = () => {
    const [toggle, setToggle] = useState(false);

    const [notifyMessage, setNotifyMessage] = useState<NotifyMessage>(
        {
            icon: "circle-check",
            title: "Success",
        }
    );

    return {
        toggle,
        setToggle,
        notifyMessage,
        setNotifyMessage,
    }
}