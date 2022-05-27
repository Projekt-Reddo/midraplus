import * as React from "react";
import { Fragment } from "react";
import { Menu, Transition } from "@headlessui/react";
import "styles/Setting.css";
import { useNavigate } from "react-router-dom";
import Icon from "components/Icon";
import { useDispatch, useSelector } from "react-redux";
import { DISCONNECT_SIGNALR, logout } from "store/actions";

interface AvatarProps {}

interface AvatarItem {
    icon?: string;
    name: string;
    className: string;
    style: object;
    isClickable: boolean;
    onClick: () => void;
}

interface AvatarItems {
    username: AvatarItem;
    email: AvatarItem;
    logout: AvatarItem;
}

const Avatar: React.FC<AvatarProps> = () => {
    // Getting user from redux
    const user = useSelector((state: any) => state.user.user);
    const onlineUsers = useSelector((state: any) => state.onlineUsers);

    // Getting current path from url
    const navigate = useNavigate();

    // Use distpatch to update redux
    const dispatch = useDispatch();

    const AvatarItems: AvatarItems = {
        username: {
            name: user.name,
            isClickable: false,
            style: {},
            className: "text-lg tracking-wide",
            onClick: () => {},
        },
        email: {
            name: user.email,
            style: {},
            className: "font-semibold text-xs",
            isClickable: false,
            onClick: () => {},
        },
        logout: {
            icon: "right-from-bracket",
            name: "Logout",
            style: { color: "#33CDFF" },
            className: "font-semibold",
            isClickable: true,
            onClick: () => {
                dispatch(logout());
                dispatch({ type: DISCONNECT_SIGNALR });
                navigate("/");
            },
        },
    };

    return (
        <>
            <Menu as="div">
                <div>
                    <div className="flex -space-x-3 fixed origin-top-right right-[144px] top-7 overflow-hidden drop-shadow-md">
                        {onlineUsers.length > 4 ? (
                            <div
                                style={{
                                    backgroundColor: "#3d3d3d",
                                    color: "white",
                                }}
                                className="inline-block rounded-full h-12 w-12 border-2 "
                            >
                                <div className="text-center pt-2 pr-2">
                                    +{onlineUsers.length - 4}
                                </div>
                            </div>
                        ) : (
                            ""
                        )}
                        {onlineUsers
                            .filter(
                                (userOnline: any) => userOnline.id !== user.id
                            )
                            .slice(0, 3)
                            .map((userOnline: any) => (
                                <div
                                    key={userOnline.id}
                                    style={{
                                        backgroundImage: `url(${userOnline.avatar})`,
                                        backgroundSize: "cover",
                                    }}
                                    className="inline-block rounded-full h-12 w-12 border-2"
                                />
                            ))}
                    </div>
                    <Menu.Button className="rounded-full fixed origin-top-right right-[6.75rem] top-7 setting-btn drop-shadow-md">
                        <div className="avatar">
                            <img
                                className="inline-block rounded-full border-2"
                                src={user.avatar}
                                alt="user avatar"
                            />
                        </div>
                    </Menu.Button>
                </div>

                <Transition
                    as={Fragment}
                    enter="transition ease-out duration-100"
                    enterFrom="transform opacity-0 scale-95"
                    enterTo="transform opacity-100 scale-100"
                    leave="transition ease-in duration-75"
                    leaveFrom="transform opacity-100 scale-100"
                    leaveTo="transform opacity-0 scale-95"
                >
                    <Menu.Items
                        className="origin-top-right fixed right-10 top-20 mt-2 w-52 px-2 rounded-md shadow-lg 
                    setting-item divide-y divide-gray-400 ring-1 ring-black ring-opacity-5 focus:outline-none
                    "
                    >
                        <div className="py-1">
                            <Menu.Item>
                                <div
                                    className={`cursor-pointer overflow-hidden whitespace-nowrap text-ellipsis text-slate-100 px-2 pt-1 text-right`}
                                >
                                    <p
                                        className={`w-100 inline ${AvatarItems.username.className}`}
                                        style={AvatarItems.username.style}
                                    >
                                        {AvatarItems.username.name}
                                    </p>
                                </div>
                            </Menu.Item>
                            <Menu.Item>
                                <div
                                    className={`cursor-pointer overflow-hidden whitespace-nowrap text-ellipsis text-slate-100 px-2 pb-1 text-right`}
                                >
                                    <p
                                        className={`w-100 inline ${AvatarItems.email.className}`}
                                        style={AvatarItems.email.style}
                                    >
                                        {AvatarItems.email.name}
                                    </p>
                                </div>
                            </Menu.Item>
                        </div>
                        <div className="py-1">
                            <Menu.Item>
                                {({ active }) => (
                                    <div
                                        className={`cursor-pointer overflow-hidden whitespace-nowrap text-ellipsis px-2 py-2 text-right`}
                                        onClick={AvatarItems.logout.onClick}
                                    >
                                        <Icon
                                            className="mr-4"
                                            icon={AvatarItems.logout.icon}
                                            color="#33CDFF"
                                            size="lg"
                                        />
                                        <p
                                            className={`w-100 inline 
                                                ${AvatarItems.logout.className}`}
                                            style={AvatarItems.logout.style}
                                        >
                                            {AvatarItems.logout.name}
                                        </p>
                                    </div>
                                )}
                            </Menu.Item>
                        </div>
                    </Menu.Items>
                </Transition>
            </Menu>
        </>
    );
};

export default Avatar;
