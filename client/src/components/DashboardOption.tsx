// Libs
import * as React from "react";
import { useNavigate, useLocation } from "react-router-dom";

// Combonent
import Icon from "components/Icon";

// Styles
import "styles/DashboardOption.css";

interface DashboardOptionProps {}

const TapItems = [
    {
        id: 1,
        icon: "tachometer-alt",
        name: "Dashboard",
        path: "/admin",
    },
    {
        id: 2,
        icon: "user-cog",
        name: "User",
        path: "/admin/user",
    },
];

const DashboardOption: React.FC<DashboardOptionProps> = () => {
    const navigate = useNavigate();
    const location = useLocation();

    return (
        <>
            <div className="dashboard-item h-screen fixed text-white">
                {/* Logo */}
                <div className="text-center mt-4">
                    <div
                        style={{
                            backgroundImage: `url(/favicon.ico)`,
                            backgroundSize: "cover",
                            backgroundPosition: "center",
                            height: "50px",
                            width: "50px",
                            margin: "0 auto",
                        }}
                    />
                    <div className="text-4xl">
                        <b>Draplus</b>
                    </div>
                </div>
                {/* Navigate Tab */}
                <div className="mt-16">
                    {TapItems.map((tap) => (
                        <div
                            key={tap.id}
                            className={
                                location.pathname === tap.path
                                    ? "tapSelectActive rounded-[8px] h-12 text-xl items-center pt-1 mb-1"
                                    : "tapSelect rounded-[8px] h-12 text-xl items-center pt-1 mb-1"
                            }
                            onClick={(e) => {
                                e.preventDefault();
                                navigate(tap.path);
                            }}
                        >
                            <Icon icon={tap.icon} className="mx-4" />
                            {tap.name}
                        </div>
                    ))}
                </div>
            </div>
        </>
    );
};
export default DashboardOption;
