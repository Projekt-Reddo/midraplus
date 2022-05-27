import * as React from "react";
import Icon from "components/Icon";
import moment from "moment";
import { API, DefaultDay } from "utils/constant";
import { Menu, Transition } from "@headlessui/react";
import Notification from "components/Notification";
import { useNavigate } from "react-router-dom";
import { useMutation } from "react-query";
import { useNotification } from "utils/useNotification";
import { useModal } from "utils/useModal";
import Modal from "components/Modal";
import ChangeTitleModal from "components/ChangeTitleModal";
import "styles/InputChangeTitle.css";
import axios from "utils/axiosInstance";

interface BoardCardProps {
    id: string;
    name?: string;
    createdAt?: string;
    lastEdit?: string;
    img?: string;
    refetch?: () => void;
}

const BoardCard: React.FC<BoardCardProps> = ({
    id,
    name,
    createdAt,
    lastEdit,
    img,
    refetch,
}) => {
    const nagivate = useNavigate();

    const goToBoard = () => {
        nagivate(`/board/${id}`);
    };

    return (
        <>
            <div className="max-w-sm h-96 relative overflow-hidden shadow-lg border border-gray-300 rounded-2xl">
                {img ? (
                    <img
                        className="w-full h-[73%] object-cover cursor-pointer"
                        src={img}
                        alt="Board"
                        onClick={goToBoard}
                    />
                ) : (
                    <div
                        className="w-full h-[73%] bg-[color:var(--bg)] cursor-pointer"
                        onClick={goToBoard}
                    />
                )}

                <div className="w-full h-[27%] px-6 py-4 bg-white flex flex- align-middle">
                    <div className="flex flex-col justify-center w-4/5">
                        <div className="font-bold text-xl mb-2 overflow-hidden">
                            {name ? name : "Last edited"}
                        </div>
                        <p className="text-gray-500 text-base">
                            <Icon icon="clock" className="mr-2" />
                            {moment(
                                lastEdit === DefaultDay ? createdAt : lastEdit
                            ).format("DD MMMM YYYY")}
                        </p>
                    </div>
                    <div className="w-1/5">
                        <BoardCardOptions
                            id={id}
                            refetch={refetch}
                            boardName_={name}
                        />
                    </div>
                </div>
            </div>
        </>
    );
};

export default BoardCard;

interface BoardCardOptionsProps {
    id: string;
    refetch?: () => void;
    boardName_?: string;
}

interface Option {
    icon: string;
    label: string;
    onClick?: () => void;
}

const BoardCardOptions: React.FC<BoardCardOptionsProps> = ({
    id,
    refetch,
    boardName_,
}) => {
    const Options: Option[] = [
        {
            icon: "trash",
            label: "Delete",
            onClick: () => {
                setOpenDelete(true);
            },
        },
        {
            icon: "pen",
            label: "Title",
            onClick: () => {
                setOpenBoardName(true);
            },
        },
    ];
    // State manage board title
    const [boardName, setBoardName] = React.useState(boardName_);

    // State manage Modal confirm
    const { open: openDelete, setOpen: setOpenDelete } = useModal();
    const { open: openBoardName, setOpen: setOpenBoardName } = useModal();
    // const
    // State manage Notification component
    const { toggle, setToggle, notifyMessage, setNotifyMessage } =
        useNotification();

    // Delete board
    const deleteBoardMutation = useMutation(async () => {
        const { data } = await axios({
            method: "DELETE",
            url: `${API}/api/board/${id}`,
        });

        return data;
    });

    // Change board title
    const changeBoardNameMutation = useMutation(async () => {
        const { data } = await axios({
            method: "PUT",
            url: `${API}/api/board/${id}`,
            data: {
                Id: id,
                Name: boardName,
            },
        });

        return data;
    });

    // Show notification when deleted
    React.useEffect(() => {
        if (deleteBoardMutation.isSuccess) {
            if (deleteBoardMutation.data.status === 200) {
                setNotifyMessage({
                    icon: "circle-check",
                    iconColor: "text-emerald-400",
                    title: "Delete board successfully",
                });

                if (refetch) refetch();
            } else {
                setNotifyMessage({
                    icon: "circle-exclamation",
                    iconColor: "text-red-400",
                    title: "Delete board failed",
                });
            }

            setToggle(true);
        }

        if (deleteBoardMutation.isError) {
            setNotifyMessage({
                icon: "circle-exclamation",
                iconColor: "text-red-400",
                title: "There is an error with server",
            });

            setToggle(true);
        }
    }, [deleteBoardMutation.isSuccess, deleteBoardMutation.isError]);

    // Show notification when changed
    React.useEffect(() => {
        if (changeBoardNameMutation.isSuccess) {
            if (changeBoardNameMutation.data.status === 200) {
                setNotifyMessage({
                    icon: "circle-check",
                    iconColor: "text-emerald-400",
                    title: "Change Board Name successfully",
                });

                if (refetch) refetch();
            } else {
                setNotifyMessage({
                    icon: "circle-exclamation",
                    iconColor: "text-red-400",
                    title: "Change Board Name failed",
                });
            }

            setToggle(true);
        }
    }, [changeBoardNameMutation.isSuccess, changeBoardNameMutation.isError]);

    return (
        <>
            <Menu as="div">
                <div>
                    <Menu.Button className="rounded-full absolute origin-bottom-right right-8 bottom-9 z-40 drop-shadow-md text-gray-700">
                        <Icon icon="ellipsis-vertical" fontSize="1.25rem" />
                    </Menu.Button>
                </div>

                <Transition
                    as={React.Fragment}
                    enter="transition ease-out duration-100"
                    enterFrom="transform opacity-0 scale-95"
                    enterTo="transform opacity-100 scale-100"
                    leave="transition ease-in duration-75"
                    leaveFrom="transform opacity-100 scale-100"
                    leaveTo="transform opacity-0 scale-95"
                >
                    <Menu.Items className="origin-bottom-right absolute right-2 bottom-20 mt-2 w-48 px-2 rounded-2xl shadow-lg bg-white divide-y divide-gray-400 ring-1 ring-black ring-opacity-5 focus:outline-none">
                        <div className="py-1">
                            {Options.map((option, index) => (
                                <Menu.Item key={index}>
                                    {({ active }) => (
                                        <div
                                            className={`text-xl cursor-pointer
                  ${active ? "text-gray-900" : "text-gray-600"}
                   block px-4 py-3`}
                                            onClick={
                                                option.onClick
                                                    ? option.onClick
                                                    : () => {}
                                            }
                                        >
                                            <Icon
                                                className="mr-3"
                                                icon={option.icon}
                                                size="lg"
                                            />
                                            {option.label}
                                        </div>
                                    )}
                                </Menu.Item>
                            ))}
                        </div>
                    </Menu.Items>
                </Transition>
            </Menu>

            <Modal
                type="error"
                open={openDelete}
                setOpen={setOpenDelete}
                title="Delete board"
                message="Are you sure you want to delete this board? Board data will be permanently removed. This action cannot be undone."
                confirmTitle="Delete"
                onConfirm={() => {
                    deleteBoardMutation.mutate();
                }}
            />

            <ChangeTitleModal
                type="info"
                open={openBoardName}
                setOpen={setOpenBoardName}
                title="Changle Board Title"
                message=""
                modalBody={
                    <div className="w-[25rem] rounded p-2 border-solid border-2 border-sky-500s">
                        <input
                            className="input-change-title w-[24rem]"
                            value={boardName == null ? "" : boardName}
                            onChange={(e) => {
                                setBoardName(e.target.value);
                            }}
                        ></input>
                    </div>
                }
                confirmTitle="Rename Board"
                onConfirm={() => {
                    changeBoardNameMutation.mutate();
                }}
            />

            <Notification
                icon={notifyMessage.icon}
                iconColor={notifyMessage.iconColor}
                title={notifyMessage.title}
                toggle={toggle}
                setToggle={setToggle}
            />
        </>
    );
};
