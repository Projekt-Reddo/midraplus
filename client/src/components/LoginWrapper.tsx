import React from "react";
import LoginButton from "./LoginButton";

import Loading from "components/Loading";
import Notification from "components/Notification";

interface LoginWrapperProps {
    googleResponse: any;
    mutation: any;
    toggle: boolean;
    setToggle: (toggle: boolean) => void;
}
const LoginWrapper: React.FC<LoginWrapperProps> = ({
    googleResponse,
    mutation,
    toggle,
    setToggle,
}) => {
    return (
        <div
            className="h-1/2 w-1/4 flex flex-col items-center justify-between border-rounded app-shadow"
            style={{
                backgroundColor: "var(--element-bg)",
                minWidth: "350px",
                maxWidth: "500px",
            }}
        >
            <div className="h-auto mx-2 content-center text-center flex flex-col justify-between items-center">
                <img
                    src="/logo192.png"
                    alt="logo"
                    className="mx-2 mt-6 mb-2 w-10 h-10"
                />
                <p className="text-2xl text-white font-bold">
                    Login To Draplus
                </p>
            </div>
            {mutation.isLoading ? (
                <Loading></Loading>
            ) : toggle ? (
                <Notification
                    icon="circle-exclamation"
                    iconColor="text-red-400"
                    title="Login failed"
                    message="Please try again"
                    toggle={toggle}
                    setToggle={() => setToggle(!toggle)}
                />
            ) : (
                <LoginButton googleResponse={googleResponse}></LoginButton>
            )}
            <div
                className="mx-2 mb-9 content-center text-center"
                style={{ color: "var(--text-small)", fontSize: "60%" }}
            >
                <p>
                    By using Draplus you are agreeing to our <br /> terms of
                    services and privacy policy
                </p>
            </div>
        </div>
    );
};

export default LoginWrapper;
