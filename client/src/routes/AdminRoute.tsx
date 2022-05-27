import * as React from "react";
import { RootStateOrAny, useSelector } from "react-redux";
import { Navigate, Outlet } from "react-router-dom";

interface AdminRouteProps {}

const AdminRoute: React.FC<AdminRouteProps> = () => {
    const user = useSelector((state: RootStateOrAny) => state.user);

    if (!user.isAuthenticated) {
        return <Navigate to="/" />;
    }

    if (!user.user?.isAdmin) {
        return <Navigate to="/board" />;
    }

    return <Outlet />;
};

export default AdminRoute;
