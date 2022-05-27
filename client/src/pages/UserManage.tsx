// Libs
import * as React from "react";
import { useQuery } from "react-query";
import { API } from "utils/constant";
import axios from "utils/axiosInstance";
import moment from "moment";

// Components
import Avatar from "components/Avatar";
import Setting from "components/Setting";
import DashboardOption from "components/DashboardOption";
import TitleAdmin from "components/TitleAdmin";
import Table from "components/Table";
import Loading from "components/Loading";
import Pagination from "components/Pagination";

// Styles
import "styles/UserManage.css";
import ButtonBanUser from "components/ButtonBanUser";

interface UserManageProps {}

const TableColumns: string[] = ["Name", "Email", "Issuer", "Created", "Action"];

const UserManage: React.FC<UserManageProps> = () => {
    const [currentPage, setCurrentPage] = React.useState(1);
    const [searchName, setSearchName] = React.useState("");

    // Get users from api
    const { isFetching, isError, data, refetch } = useQuery(
        "users",
        async () => {
            const { data } = await axios.get(
                getUrltoFetch(currentPage, searchName)
            );
            return data;
        },
        {
            enabled: false,
        }
    );

    React.useEffect(() => {
        refetch();
    }, [currentPage]);

    return (
        <>
            <DashboardOption />
            <TitleAdmin />
            <div className="userManage text-white">
                <div className="mt-4">
                    <div className="text-3xl">User</div>
                    <div>Admin {`>`} User</div>
                </div>
                {/* Table */}
                {isFetching || !data ? (
                    <div className="w-full h-[calc(100vh-18rem)] flex justify-center items-center">
                        <Loading />
                    </div>
                ) : isError ? (
                    <div className="w-full h-[calc(100vh-18rem)] flex justify-center items-center">
                        <div>error</div>
                    </div>
                ) : (
                    <>
                        <Table
                            columns={TableColumns}
                            data={data?.payload.map((user: any) => ({
                                fullname: (
                                    <div className="flex pl-4 mx-2">
                                        <div
                                            style={{
                                                backgroundImage: `url(${user.avatar})`,
                                                backgroundSize: "cover",
                                                backgroundPosition: "center",
                                                height: "50px",
                                                width: "50px",
                                                borderRadius: "50%",
                                                border: "2px solid white",
                                                minWidth: "50px",
                                            }}
                                        />
                                        <div className="self-center ml-2">
                                            {user.name}
                                        </div>
                                    </div>
                                ),
                                email: <div className="mx-2">{user.email}</div>,
                                issuer: (
                                    <div className="mx-2">{user.issuer}</div>
                                ),
                                created: (
                                    <div className="mx-2">
                                        {moment(user.createdAt).format(
                                            "YYYY MMM Do"
                                        )}
                                    </div>
                                ),
                                action: (
                                    <ButtonBanUser
                                        user={user}
                                        refetch={refetch}
                                    />
                                ),
                            }))}
                            style={{ width: "100%" }}
                            className="flex justify-center items-center"
                        />

                        <Pagination
                            totalRecords={data.totalRecords}
                            currentPage={currentPage}
                            pageSize={pageSize}
                            onPageChange={setCurrentPage}
                        />
                    </>
                )}
            </div>
            <Setting />
            <Avatar />
        </>
    );
};

export default UserManage;

const pageSize = 10;
const getUrltoFetch = (page: any, keyword: string) => {
    return `${API}/api/User?pageSize=${pageSize}&pageNumber=${page}&searchName=${keyword}`;
};
