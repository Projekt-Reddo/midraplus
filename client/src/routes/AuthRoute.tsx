import * as React from "react";
import { RootStateOrAny, useSelector } from "react-redux";
import { Navigate, Outlet } from "react-router-dom";

interface AuthRouteProps {}

const AuthRoute: React.FC<AuthRouteProps> = () => {
    const user = useSelector((state: RootStateOrAny) => state.user);

    if (user.isAuthenticated) {
        return <Navigate to="/board" />;
    }

    return <Outlet />;
};

export default AuthRoute;
