import * as React from "react";
import ReactDOM from "react-dom";
import Icon from "components/Icon";
import { Transition } from "@headlessui/react";

interface NotificationProps {
    icon: string;
    iconColor?: string;
    title: string;
    message?: string;
    toggle: boolean;
    setToggle: (toggle: boolean) => void;
    hideAfter?: number;
}

const Notification: React.FC<NotificationProps> = ({
    icon,
    iconColor = "text-emerald-400",
    title,
    message,
    toggle,
    setToggle,
    hideAfter = 2.2,
}) => {
    React.useEffect(() => {
        if (toggle) {
            setTimeout(() => {
                setToggle(false);
            }, 1000 * hideAfter);
        }
    }, [toggle]);

    return ReactDOM.createPortal(
        <Transition
            enter="transition ease-out duration-100"
            enterFrom="transform opacity-0 scale-95"
            enterTo="transform opacity-100 scale-100"
            leave="transition ease-in duration-75"
            leaveFrom="transform opacity-100 scale-100"
            leaveTo="transform opacity-0 scale-95"
            show={toggle}
        >
            {toggle && (
                <div
                    className="fixed bottom-3 left-0 right-0 mx-auto w-fit rounded-lg border-gray-300 p-3 drop-shadow-md"
                    style={{ backgroundColor: "var(--element-bg)" }}
                    onClick={() => {
                        setToggle(false);
                    }}
                >
                    <div className="flex flex-row">
                        <div className={`px-2 ${iconColor}`}>
                            <Icon icon={icon} size="lg" />
                        </div>
                        <div className="ml-2 mr-6">
                            <span className="font-semibold text-slate-50">
                                {title}
                            </span>
                            <span className="block text-slate-200">
                                {message}
                            </span>
                        </div>
                    </div>
                </div>
            )}
        </Transition>,
        document.body
    );
};

export default Notification;
