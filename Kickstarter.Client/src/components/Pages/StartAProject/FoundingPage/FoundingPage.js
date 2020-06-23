import React, { useEffect } from "react";
import Divider from "@material-ui/core/Divider";
import {CustomButton} from "../../../Decorators/Buttons";
import GoalCol from "./GoalCol";
import DurationCol from "./DurationCol";
import PageTitle from '../PageTitle';
import ColSubscribe from '../ColSubscribe';
import { updateEndDate, updateGoal, loadFunding } from "../../../../modules/actions/UpdateFoundingPage";
import { useDispatch, useSelector } from "react-redux";
import { FundingEndpoint } from '../../../../Helpers/AppRoutes';
import { useSnackbar } from "notistack";
import { CreateRequestOptions, GetAuthHeader } from '../../../../Helpers/ApiCall';


export const FoundingPage = (props) => {
    const funding = useSelector(state => state.FoundingPageReducer);
    const dispatch = useDispatch();
    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        const loadPage = async () => {
            let params = new URLSearchParams({
                id: props.companyId,
                auth: true,
            });
            let response = await fetch(FundingEndpoint.GetFunding + params.toString(), {
                headers: GetAuthHeader(),
                method: 'get'
            });

            if (isResponseOk(response)){
                let result = await response.json();
                console.log(result)
                dispatch(updateGoal(result.goal));
                if (result.endFunding !== null) {
                    dispatch(updateEndDate(result.endFunding));
                }
            }
        }

        loadPage();
    }, [])
    
    const isResponseOk = (response, method) => {
        if (response.ok){
            if (method === 'post'){
                showSnackbar('Saved successful', 'success')
            }

            return true;
        }

        if (response.status >= 500) {
            showSnackbar('Unable to load data, server side error. Status: ' + response.status, 'info');
        }
        else if (response.status >= 400) {
            showSnackbar('Client error. Status: ' + response.status, 'warning');
        }
        else if (response.status >= 300) {
            showSnackbar('Redirect missing. Status: ' + response.status, 'warning');
        }

        if (method === 'post'){
            showSnackbar('Unable to save. Status: ' + response.status, 'error');
        }

        return false;
    }

    const showSnackbar = (message, variant) => {
        enqueueSnackbar(message, { variant: variant });
    }

    const onSaveClicked = async () => {
        let data = {
            Id: props.companyId,
            goal: funding.goal,
            endDate: funding.endDate.format(),
        }

        console.log(data)

        let response = await fetch(FundingEndpoint.Update, CreateRequestOptions(data, 'post', true));
        console.log(response);
    }

    return(
        <div>
            <PageTitle title={' Let’s talk about money'} subtitle={'Plan and manage your project’s finances.'}/>
            <div className="row w-100 my-5 mx-auto">
                <ColSubscribe
                    title={'Funding goal'}
                    description={[
                        'Set an achievable goal that covers what you need to complete your project.',
                        'Funding is all-or-nothing. If you don’t meet your goal, you won’t receive any money.',
                    ]}/>
                <GoalCol 
                    goal={funding.goal} 
                    onGoalChange={(amount) => dispatch(updateGoal(amount))} 
                    className="col ml-auto pt-1"/>
            </div>
            <Divider className="mt-4"/>
            <div className="row w-100 my-5 mx-auto">
                <ColSubscribe
                    title={'Campaign duration'}
                    description={[
                        'Set a time limit for your campaign. You won’t be able to change this after you launch.'
                    ]}
                />
                <DurationCol
                    endDate={funding.endDate}
                    className="col ml-auto pt-1"
                    onEndDateChange={(date) => dispatch(updateEndDate(date))}
                />
            </div>
            <Divider className="mt-4"/>
            <div className="my-4 row">
                <CustomButton variant="contained" size="medium" color="primary" 
                              className="mx-auto" onClick={() => onSaveClicked()}>
                    Save
                </CustomButton>
            </div>
        </div>
    )
}