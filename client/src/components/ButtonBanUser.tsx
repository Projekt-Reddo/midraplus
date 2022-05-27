import * as React from "react";
import Icon from "./Icon";
import { useMutation } from "react-query";
import axios from "utils/axiosInstance";
import { API } from "utils/constant";
import { useModal } from "utils/useModal";
import Modal from "./Modal";

interface ButtonBanUserProps {
    user: any;
    refetch: any;
}

const ButtonBanUser: React.FC<ButtonBanUserProps> = ({ user, refetch }) => {
    const banUserMutation = useMutation(
        async (userId: string) => {
            const { data } = await axios({
                method: "put",
                url: `${API}/api/user/ban/${userId}`,
            });

            return data;
        },
        {
            onSuccess: () => {
                refetch();
            },
        }
    );

    const { open, setOpen } = useModal();

    if (user.isBanned) {
        return (
            <>
                <div
                    className="iconBan"
                    style={{
                        cursor: !banUserMutation.isLoading
                            ? "pointer"
                            : "default",

                        opacity: banUserMutation.isLoading ? 0.5 : 1,
                    }}
                    onClick={() => setOpen(true)}
                >
                    <Icon icon="ban" className="mt-[9px]" />
                </div>

                <Modal
                    type="info"
                    open={open}
                    setOpen={setOpen}
                    title="Unban user"
                    message="Are you sure you want to unban this user?"
                    confirmTitle="Unban"
                    onConfirm={() => {
                        banUserMutation.mutate(user.id);
                    }}
                />
            </>
        );
    }

    return (
        <>
            <div
                className="iconLock"
                style={{
                    cursor: !banUserMutation.isLoading ? "pointer" : "default",

                    opacity: banUserMutation.isLoading ? 0.5 : 1,
                }}
                onClick={() => setOpen(true)}
            >
                <Icon icon="lock-open" className="mt-[9px]" />
            </div>

            <Modal
                type="error"
                open={open}
                setOpen={setOpen}
                title="Ban user"
                message="Are you sure you want to ban this user?"
                confirmTitle="Ban"
                onConfirm={() => {
                    banUserMutation.mutate(user.id);
                }}
            />
        </>
    );
};

export default ButtonBanUser;
