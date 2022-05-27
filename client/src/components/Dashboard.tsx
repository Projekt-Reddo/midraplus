import * as React from "react";
import "styles/Dashboard.css";
import Icon from "./Icon";
import { ResponsiveBar } from "@nivo/bar";
import { ResponsiveLine } from "@nivo/line";
import axios from "utils/axiosInstance";
import { API } from "utils/constant";
import { useQuery } from "react-query";
import Loading from "components/Loading";
import { PDFExport } from "@progress/kendo-react-pdf";
import "styles/KendoPDF.css";

interface DashboardProps {}

const Dashboard: React.FC<DashboardProps> = () => {
    const pdfExportComponent = React.useRef<PDFExport>(null);

    const handleExportPdf = () => {
        pdfExportComponent.current!.save();
    };

    return (
        <>
            <div className="dashboard">
                <div className="mx-[2rem]">
                    <div className="flex flex-row justify-between items-center">
                        <div className="mt-4">
                            <div className="text-3xl">Dashboard</div>
                            <div>Admin {`>`} Dashboard</div>
                        </div>
                        <button
                            className="bg-cyan-500 hover:bg-cyan-700 text-white font-bold h-fit py-2 px-4 mt-2 rounded"
                            onClick={handleExportPdf}
                        >
                            <Icon icon="file-export" className="mr-2" />
                            Export
                        </button>
                    </div>
                    <PDFExport
                        ref={pdfExportComponent}
                        fileName={`Report`}
                        creator="Draplus"
                    >
                        <Details />
                        <div className="grid grid-flow-row lg:grid-cols-2 gap-4 mt-[2rem] ">
                            <DashboardBar />
                            <DashboardLine />
                        </div>
                    </PDFExport>
                </div>
            </div>
        </>
    );
};

export default Dashboard;

interface DashboardDetailProps {}

const Details: React.FC<DashboardDetailProps> = () => {
    const [sample, setSample] = React.useState({
        newAccount: 0,
        newBoard: 0,
        totalAccount: 0,
        totalBoard: 0,
    });

    const { isFetching, data, refetch } = useQuery(
        "dashboard",
        async () => {
            const { data } = await axios.get(
                `${API}/api/admin/dashboard/detail`
            );
            setSample(data);
            return data;
        },
        {
            enabled: false,
        }
    );

    React.useEffect(() => {
        refetch();
    }, []);

    return (
        <>
            <div className="mt-[2rem]">
                {isFetching || !data ? (
                    <div className="w-full h-screen flex justify-center items-center">
                        <Loading />
                    </div>
                ) : (
                    <div className="grid grid-flow-row sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-5 ">
                        <div
                            className=" dashboard-info flex flex-col"
                            style={{ background: "#28A5F9" }}
                        >
                            <div className="icon-contain">
                                <Icon icon="user" className="m-[0.9rem]" />
                            </div>
                            <p className="justify-start text-6xl ml-5 pt-2 mt-[2rem] ">
                                {sample.newAccount}
                            </p>
                            <p className=" justify-start  ml-5 font-bold pt-2 mb-5">
                                New Account
                            </p>
                        </div>
                        <div
                            className=" dashboard-info flex flex-col"
                            style={{ background: "#EF6F9E" }}
                        >
                            <div className="icon-contain">
                                <Icon
                                    icon="chalkboard"
                                    className="m-[0.9rem]"
                                />
                            </div>
                            <p className=" justify-start text-6xl ml-5 pt-2 mt-[2rem]">
                                {sample.newBoard}
                            </p>
                            <p className=" justify-start  ml-5 font-bold pt-2 mb-5">
                                New Board
                            </p>
                        </div>
                        <div
                            className=" dashboard-info flex flex-col"
                            style={{ background: "#FAC66D" }}
                        >
                            <div className="icon-contain">
                                <Icon icon="users" className="m-[0.9rem]" />
                            </div>
                            <p className="justify-start text-6xl ml-5 pt-2 mt-[2rem]">
                                {sample.totalAccount}
                            </p>
                            <p className=" justify-start  ml-5 font-bold pt-2 mb-5">
                                Total Accounts
                            </p>
                        </div>
                        <div
                            className=" dashboard-info flex flex-col"
                            style={{ background: "#705DBC" }}
                        >
                            <div className="icon-contain">
                                <Icon icon="clipboard" className="m-[0.9rem]" />
                            </div>
                            <p className=" justify-start text-6xl ml-5 pt-2 mt-[2rem]">
                                {sample.totalBoard}
                            </p>
                            <p className=" justify-start  ml-5 font-bold pt-2 mb-5">
                                Total Boards
                            </p>
                        </div>
                    </div>
                )}
            </div>
        </>
    );
};

interface DashboardBarProps {}

const DashboardBar: React.FunctionComponent<DashboardBarProps> = () => {
    const { isFetching, data, refetch } = useQuery(
        "dashboardbar",
        async () => {
            const { data } = await axios.get(
                `${API}/api/admin/dashboard/bar/${selectedOption}`
            );
            setSample(data);
            return data;
        },
        {
            enabled: false,
        }
    );

    const [sample, setSample] = React.useState([
        {
            country: "string",
            login: 0,
            loginColor: "string",
        },
    ]);

    const selectChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const value = event.target.value;
        setSelectedOption(value);
    };

    const [selectedOption, setSelectedOption] = React.useState("Day");

    React.useEffect(() => {
        refetch();
    }, [selectedOption]);

    return (
        <>
            <div className="dashboard-chart ">
                {isFetching || !data ? (
                    <div className="w-full h-full flex justify-center items-center">
                        <Loading />
                    </div>
                ) : (
                    <div className="dashboard-chart ">
                        <div className="flex">
                            <p className="justify-start text-chart ml-7 font-bold pt-2">
                                Logged In
                            </p>
                            <div className="justify-end select-time ">
                                <select
                                    onChange={selectChange}
                                    value={selectedOption}
                                >
                                    <option value="Day">Day</option>
                                    <option value="Month">Month</option>
                                    <option value="Year">Year</option>
                                </select>
                            </div>
                        </div>
                        <ResponsiveBar
                            data={sample}
                            keys={["login"]}
                            indexBy="country"
                            margin={{
                                top: 40,
                                right: 50,
                                bottom: 70,
                                left: 60,
                            }}
                            padding={0.4}
                            valueScale={{ type: "linear" }}
                            indexScale={{ type: "band", round: true }}
                            colors={{ scheme: "nivo" }}
                            axisLeft={{
                                tickSize: 5,
                                tickPadding: 5,
                                tickRotation: 0,
                                legendPosition: "middle",
                                legendOffset: -40,
                            }}
                            isInteractive={false}
                            defs={[
                                {
                                    id: "dots",
                                    type: "patternDots",
                                    background: "inherit",
                                    color: "#38bcb2",
                                    size: 4,
                                    padding: 1,
                                    stagger: true,
                                },
                                {
                                    id: "lines",
                                    type: "patternLines",
                                    background: "inherit",
                                    color: "#eed312",
                                    rotation: -45,
                                    lineWidth: 6,
                                    spacing: 10,
                                },
                            ]}
                            borderRadius={4}
                            borderColor={{
                                from: "color",
                                modifiers: [["darker", 1.2]],
                            }}
                            barAriaLabel={function (e) {
                                return (
                                    e.id +
                                    ": " +
                                    e.formattedValue +
                                    " in country: " +
                                    e.indexValue
                                );
                            }}
                        />
                    </div>
                )}
            </div>
        </>
    );
};

interface DashboardLineProps {}

const DashboardLine: React.FunctionComponent<DashboardLineProps> = () => {
    const [sample, setSample] = React.useState([
        {
            id: "string",
            color: "string",
            data: [
                {
                    x: "string",
                    y: 0,
                },
            ],
        },
    ]);

    const { isFetching, data, refetch } = useQuery(
        "dashboardline",
        async () => {
            const { data } = await axios.get(
                `${API}/api/admin/dashboard/line/${selectedLine}`
            );
            setSample(data);
            return data;
        },
        {
            enabled: false,
        }
    );

    const [selectedLine, setselectedLine] = React.useState("Day");
    const selectLine = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const value = event.target.value;
        setselectedLine(value);
    };

    React.useEffect(() => {
        refetch();
    }, [selectedLine]);

    return (
        <>
            <div className="dashboard-chart ">
                {isFetching || !data ? (
                    <div className="w-full h-full flex justify-center items-center">
                        <Loading />
                    </div>
                ) : (
                    <div className="dashboard-chart ">
                        <div className="flex">
                            <p className="justify-start text-chart ml-7 font-bold pt-2">
                                Board
                            </p>
                            <div className="justify-end select-time mr-1">
                                <select
                                    onChange={selectLine}
                                    value={selectedLine}
                                >
                                    <option value="Day">Day</option>
                                    <option value="Month">Month</option>
                                    <option value="Year">Year</option>
                                </select>
                            </div>
                        </div>
                        <ResponsiveLine
                            data={sample}
                            margin={{
                                top: 40,
                                right: 50,
                                bottom: 70,
                                left: 60,
                            }}
                            xScale={{ type: "point" }}
                            yScale={{
                                type: "linear",
                                min: "auto",
                                max: "auto",
                                stacked: true,
                                reverse: false,
                            }}
                            isInteractive={false}
                            yFormat=" >-.2f"
                            curve="basis"
                            colors={{ scheme: "nivo" }}
                            lineWidth={6}
                            enablePoints={false}
                            pointColor={{ theme: "background" }}
                            pointBorderWidth={2}
                            pointBorderColor={{ from: "serieColor" }}
                            pointLabelYOffset={-12}
                            useMesh={true}
                            legends={[]}
                        />
                    </div>
                )}
            </div>
        </>
    );
};
