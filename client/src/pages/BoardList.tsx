import * as React from "react";
import BoardCard from "components/BoardCard";
import Setting from "components/Setting";
import Icon from "components/Icon";
import { useMutation, useQuery } from "react-query";
import { API } from "utils/constant";
import Loading from "components/Loading";
import { useSelector } from "react-redux";
import Notification from "components/Notification";
import { useNotification } from "utils/useNotification";
import Avatar from "components/Avatar";
import axios from "utils/axiosInstance";

interface BoardListProps {}

interface Board {
    id: string;
    name?: string;
    createdAt?: string;
    lastEdit?: string;
    img?: string;
    chatRoomId?: string;
}

const BoardList: React.FC<BoardListProps> = () => {
    // Get user information from store
    const userFromStore = useSelector((state: any) => state.user);

    // Get boards from api
    const { isLoading, isError, data, refetch } = useQuery(
        "boards",
        async () => {
            const { data } = await axios.get(
                `${API}/api/board/${userFromStore.user.id}`
            );
            return data;
        },
        {
            enabled: false,
        }
    );

    React.useEffect(() => {
        refetch();
    }, []);

    return (
        <div className="max-w-full min-h-screen bg-[color:var(--bg)]">
            {isLoading || !data ? (
                <div className="w-full h-screen flex justify-center items-center">
                    <Loading />
                </div>
            ) : isError ? (
                <div className="w-full h-screen flex justify-center items-center">
                    <div>error</div>
                </div>
            ) : (
                <div className="grid grid-flow-row sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-3 2xl:grid-cols-4 gap-8 pt-24 pb-8 px-20">
                    {/* Create new board */}
                    <CreateBoard refetch={refetch} />

                    {/* User boards */}
                    {data.map((board: Board) => (
                        <BoardCard
                            key={board.id}
                            id={board.id}
                            name={board.name}
                            createdAt={board.createdAt}
                            lastEdit={board.lastEdit}
                            img={board.img}
                            refetch={refetch}
                        />
                    ))}
                </div>
            )}

            <Setting />
            <Avatar />
        </div>
    );
};

export default BoardList;

interface CreateBoardProps {
    refetch: () => void;
}

const CreateBoard: React.FC<CreateBoardProps> = ({ refetch }) => {
    // Get user from store
    const userFromStore = useSelector((state: any) => state.user);

    // Notification and Message
    const { toggle, setToggle, notifyMessage, setNotifyMessage } =
        useNotification();

    // Create new board
    const createBoardMutation = useMutation(async (newBoard: object) => {
        const { data } = await axios({
            method: "post",
            url: `${API}/api/board`,
            data: newBoard,
        });

        return data;
    });

    // Show notification when created
    React.useEffect(() => {
        if (createBoardMutation.isSuccess) {
            if (createBoardMutation.data.status === 200) {
                setNotifyMessage({
                    icon: "circle-check",
                    iconColor: "text-emerald-400",
                    title: "Create board successfully",
                });

                // refresh data
                refetch();
            } else {
                setNotifyMessage({
                    icon: "circle-exclamation",
                    iconColor: "text-red-400",
                    title: "Create board failed",
                });
            }

            setToggle(true);
        }

        if (createBoardMutation.isError) {
            setNotifyMessage({
                icon: "circle-exclamation",
                iconColor: "text-red-400",
                title: "There is an error with server",
            });

            setToggle(true);
        }
    }, [createBoardMutation.isSuccess, createBoardMutation.isError]);

    return (
        <>
            <div
                className="max-w-sm h-96 shadow-lg border border-gray-300 rounded-2xl flex justify-center items-center"
                onClick={() => {
                    createBoardMutation.mutate({
                        userId: userFromStore.user.id,
                    });
                }}
            >
                <div className="rounded-full h-[3rem] w-[3rem] bg-[color:var(--element-bg)] flex justify-center items-center">
                    <Icon icon="plus" className=" text-white bold" size="xl" />
                </div>
            </div>

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
