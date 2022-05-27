import * as React from "react";
import ReactDOM from "react-dom";
import { Dialog, Transition } from "@headlessui/react";
import {
    ExclamationIcon,
    ExclamationCircleIcon,
    DocumentIcon,
} from "@heroicons/react/outline";

interface ChangeTitleModalProps {
    type: "info" | "error" | "warning";
    open: boolean;
    setOpen: (open: boolean) => void;
    title: string;
    message: string;
    modalBody?: any;
    onConfirm: () => void;
    confirmTitle?: string;
    onCancel?: () => void;
    cancelTitle?: string;
}

const ChangeTitleModal: React.FC<ChangeTitleModalProps> = ({
    type = "info",
    open = false,
    setOpen,
    title,
    message,
    modalBody,
    onConfirm = () => {},
    confirmTitle = "Accept",
    onCancel = () => {},
    cancelTitle = "Cancel",
}) => {
    const cancelButtonRef = React.useRef<HTMLButtonElement>(null);

    var style = styleOptions[type];

    return ReactDOM.createPortal(
        <Transition.Root show={open} as={React.Fragment}>
            <Dialog
                as="div"
                className="fixed z-50 inset-0 overflow-y-auto"
                initialFocus={cancelButtonRef}
                onClose={setOpen}
            >
                <div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
                    <Transition.Child
                        as={React.Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0"
                        enterTo="opacity-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100"
                        leaveTo="opacity-0"
                    >
                        <Dialog.Overlay className="fixed inset-0 bg-[#232222] bg-opacity-70 transition-opacity" />
                    </Transition.Child>

                    {/* This element is to trick the browser into centering the modal contents. */}
                    <span
                        className="hidden sm:inline-block sm:align-middle sm:h-screen"
                        aria-hidden="true"
                    >
                        &#8203;
                    </span>
                    <Transition.Child
                        as={React.Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                        enterTo="opacity-100 translate-y-0 sm:scale-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100 translate-y-0 sm:scale-100"
                        leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                    >
                        <div className="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
                            <div className="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
                                <div className="sm:flex sm:items-start">
                                    <div
                                        className={`mx-auto flex-shrink-0 flex items-center justify-center h-12 w-12 rounded-full ${style.iconCover} sm:mx-0 sm:h-10 sm:w-10`}
                                    >
                                        {style.icon}
                                    </div>
                                    <div className="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left">
                                        <Dialog.Title
                                            as="h3"
                                            className="text-lg leading-6 font-medium text-gray-900"
                                        >
                                            {title}
                                        </Dialog.Title>
                                        <div className="mt-2">
                                            {modalBody}
                                            <p className="text-sm text-gray-500">
                                                {message}
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
                                <button
                                    type="button"
                                    className={`w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 ${style.btnColor} text-base font-medium text-white ${style.btnHover} focus:outline-none sm:ml-3 sm:w-auto sm:text-sm`}
                                    onClick={() => {
                                        onConfirm();
                                        setOpen(false);
                                    }}
                                >
                                    {confirmTitle}
                                </button>
                                <button
                                    type="button"
                                    className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
                                    onClick={() => {
                                        onCancel();
                                        setOpen(false);
                                    }}
                                    ref={cancelButtonRef}
                                >
                                    {cancelTitle}
                                </button>
                            </div>
                        </div>
                    </Transition.Child>
                </div>
            </Dialog>
        </Transition.Root>,
        document.body
    );
};

export default ChangeTitleModal;

const styleOptions = {
    info: {
        icon: (
            <DocumentIcon
                className={`h-6 w-6 text-cyan-600`}
                aria-hidden="true"
            />
        ),
        textColor: "text-cyan-600",
        iconCover: "bg-cyan-100",
        btnColor: "bg-cyan-600",
        btnHover: "hover:bg-cyan-700",
    },
    error: {
        icon: (
            <ExclamationCircleIcon
                className={`h-6 w-6 text-red-600`}
                aria-hidden="true"
            />
        ),
        textColor: "text-red-600",
        iconCover: "bg-red-100",
        btnColor: "bg-red-600",
        btnHover: "hover:bg-red-700",
    },
    warning: {
        icon: (
            <ExclamationIcon
                className={`h-6 w-6 text-amber-600`}
                aria-hidden="true"
            />
        ),
        textColor: "text-amber-600",
        iconCover: "bg-amber-100",
        btnColor: "bg-amber-600",
        btnHover: "hover:bg-amber-700",
    },
};
