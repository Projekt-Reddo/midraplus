import * as React from "react";
import { Fragment } from "react";
import { Menu, Transition } from "@headlessui/react";
import "styles/Setting.css";
import { useNavigate } from "react-router-dom";
import Icon from "components/Icon";
import { matchRoutes, useLocation } from "react-router-dom";
import Notification from "components/Notification";
import { RootStateOrAny, useSelector } from "react-redux";

interface SettingProps {}

interface SettingItem {
    icon: string;
    name: string;
    path: string;
}

const SettingItems: SettingItem[] = [
    {
        icon: "share-square",
        name: "Share",
        path: "/",
    },
    {
        icon: "layer-group",
        name: "My boards",
        path: "/board",
    },
    {
        icon: "bars-progress",
        name: "Admin",
        path: "/admin",
    },
    {
        icon: "question-circle",
        name: "Help",
        path: "/help",
    },
    {
        icon: "info-circle",
        name: "About",
        path: "https://github.com/Projekt-Reddo/draplus",
    },
];

const Setting: React.FC<SettingProps> = () => {
    // Getting current path from url
    const location = useLocation();

    const navigate = useNavigate();

    // State manage Notification component
    const [toggle, setToggle] = React.useState(false);

    // Show Share only in board page
    const routes = [{ path: "/board/:id" }];

    const match = matchRoutes(routes, location.pathname);

    // Show Admin only admin user
    const user = useSelector((state: RootStateOrAny) => state.user);

    return (
        <>
            <Menu as="div">
                <div>
                    <Menu.Button className="rounded-full fixed origin-top-right right-10 top-7 setting-btn drop-shadow-md">
                        <Icon icon="gear" fontSize="1.25rem" />
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
                    <Menu.Items className="origin-top-right fixed right-10 top-20 mt-2 w-52 px-2 rounded-md shadow-lg setting-item divide-y divide-gray-400 ring-1 ring-black ring-opacity-5 focus:outline-none">
                        {SettingItems.map(
                            (item: SettingItem, index: number) => {
                                if (!match && item.name === "Share") {
                                    return (
                                        <React.Fragment
                                            key={index}
                                        ></React.Fragment>
                                    );
                                }

                                if (
                                    !user.user.isAdmin &&
                                    item.name === "Admin"
                                ) {
                                    return (
                                        <React.Fragment
                                            key={index}
                                        ></React.Fragment>
                                    );
                                }

                                return (
                                    <div className="py-1" key={index}>
                                        <Menu.Item>
                                            {({ active }) => (
                                                <div
                                                    className={`setting-item cursor-pointer
                ${active ? "text-slate-400" : "text-slate-100"}
                 block px-4 py-2`}
                                                    onClick={
                                                        item.name === "Share"
                                                            ? (e) => {
                                                                  // Prevent redirect
                                                                  e.preventDefault();

                                                                  // Copy current url to clipboard
                                                                  navigator.clipboard.writeText(
                                                                      `${window.location.origin}${location.pathname}`
                                                                  );

                                                                  // Event for IE
                                                                  //  window.clipboardData.setData("Text", "https://draplus.app/");

                                                                  // Show notification
                                                                  setToggle(
                                                                      true
                                                                  );
                                                              }
                                                            : item.name ===
                                                              "About"
                                                            ? (e) => {
                                                                  e.preventDefault();
                                                                  window.open(
                                                                      `${item.path}`,
                                                                      "_blank"
                                                                  );
                                                              }
                                                            : (e) => {
                                                                  e.preventDefault();
                                                                  navigate(
                                                                      item.path
                                                                  );
                                                              }
                                                    }
                                                >
                                                    <Icon
                                                        className="mr-4"
                                                        icon={item.icon}
                                                        size="lg"
                                                    />
                                                    {item.name}
                                                </div>
                                            )}
                                        </Menu.Item>
                                    </div>
                                );
                            }
                        )}
                    </Menu.Items>
                </Transition>
            </Menu>

            <Notification
                icon="circle-check"
                title="Copy to clipboard successfully"
                toggle={toggle}
                setToggle={setToggle}
            />
        </>
    );
};

export default Setting;
