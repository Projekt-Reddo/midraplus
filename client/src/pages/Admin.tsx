import * as React from "react";

import TitleAdmin from "components/TitleAdmin";
import DashboardOption from "components/DashboardOption";
import Dashboard from "components/Dashboard";
import Avatar from "components/Avatar";
import Setting from "components/Setting";

interface AdminProps {}

const Admin: React.FC<AdminProps> = () => {
    return (
        <>
            <DashboardOption />
            <TitleAdmin />
            <Avatar />
            <Setting />
            <Dashboard />
        </>
    );
};

export default Admin;
