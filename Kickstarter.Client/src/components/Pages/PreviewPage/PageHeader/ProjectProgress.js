import {Progress} from "antd";
import React from "react";
import {GetDatesDifference, GetLocalNum} from "../../../methods";

export default function ProjectProgress(founding) {
    
    const foundedBox = () => (
        <div className="mr-box">
            <p className="founded green-main">${GetLocalNum(founding.founded ? founding.founded : 0)}</p>
            <p className="text-small text-muted ">pledged of ${GetLocalNum(founding.goal ? founding.goal : 0)} goal</p>
        </div>
    );

    const backersBox = () => (
        <div className="mx-box">
            <p className="founded text-secondary">{GetLocalNum(founding.backers ? founding.backers : 0)}</p>
            <p className="text-small text-muted ">backers</p>
        </div>
    );

    const daysBox = () => (
        <div className="mx-box">
            <p className="founded text-secondary">{GetLocalNum(GetDatesDifference(founding.duration))}</p>
            <p className="text-small text-muted">days to go</p>
        </div>
    );
    
    
    return(
        <div >
            <Progress
                percent={Number(77)}
                showInfo={false}
                size="small"/>
            <div className="text-left hide-on-md">
                {foundedBox()}
                {backersBox()}
                {daysBox()}
            </div>
            <div className="show-on-md">
                <div className="row text-left mx-auto">
                    {foundedBox()}
                    {backersBox()}
                    {daysBox()}
                </div>
            </div>
        </div>
    )
}