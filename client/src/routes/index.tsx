// Libs
import * as React from "react";
import { Route, Routes } from "react-router-dom";

// Pages
import Login from "pages/Login";
import Board from "pages/Board";
import BoardList from "pages/BoardList";
import ErrorPage from "pages/ErrorPage";
import Admin from "pages/Admin";

import { useEffect } from "react";
import { init } from "utils/loginHandlers";
import { login } from "store/actions/index";
import { useDispatch } from "react-redux";
import Loading from "components/Loading";
import AuthRoute from "./AuthRoute";
import UserRoute from "./UserRoute";
import HelpPage from "pages/HelpPage";
import UserManage from "pages/UserManage";
import AdminRoute from "./AdminRoute";
import BannedPage from "pages/BannedPage";

const BaseRoutes: React.FC = () => {
    const dispatch = useDispatch();

    const [isLoading, setIsLoading] = React.useState(true);
    useEffect(() => {
        var rs = init();

        if (rs) {
            const userStored = localStorage.getItem("user");
            const user = JSON.parse(userStored || "{}");
            const accessToken = localStorage.getItem("accessToken");
            dispatch(login({ ...user, accessToken: accessToken }));
        }

        setIsLoading(false);
    }, []);

    if (isLoading) {
        return (
            <div className="h-screen w-screen bg-[color:var(--bg)] flex items-center justify-center">
                <Loading />
            </div>
        );
    }

    return (
        <Routes>
            <Route path="/" element={<AuthRoute />}>
                <Route path="/" element={<Login />} />
            </Route>
            {/* <Route path="/" element={<UserRoute />}> */}
            <Route path="/banned" element={<BannedPage />} />
            {/* </Route> */}
            <Route path="/" element={<UserRoute />}>
                <Route path="/board/:boardId" element={<Board />} />
            </Route>
            <Route path="/" element={<UserRoute />}>
                <Route path="/board" element={<BoardList />} />
            </Route>
            <Route path="/" element={<UserRoute />}>
                <Route path="/help" element={<HelpPage />} />
            </Route>
            <Route path="/" element={<AdminRoute />}>
                <Route path="/admin" element={<Admin />} />
            </Route>
            <Route path="/" element={<AdminRoute />}>
                <Route path="/admin/user" element={<UserManage />} />
            </Route>
            <Route path="/" element={<AdminRoute />}>
                <Route path="/admin/user" element={<UserManage />} />
            </Route>
            <Route path="*" element={<ErrorPage />} />
        </Routes>
    );
};

export default BaseRoutes;
