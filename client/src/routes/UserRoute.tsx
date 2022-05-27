import * as React from "react";
import { RootStateOrAny, useSelector } from "react-redux";
import { Navigate, Outlet } from "react-router-dom";

interface UserRouteProps {}

const UserRoute: React.FC<UserRouteProps> = () => {
    const user = useSelector((state: RootStateOrAny) => state.user);

    if (!user.isAuthenticated) {
        return <Navigate to="/" />;
    }

    return <Outlet />;
};

export default UserRoute;
