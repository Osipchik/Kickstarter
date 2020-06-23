import React, { useEffect } from 'react'
import ListItem from '@material-ui/core/ListItem';
import { List } from '@material-ui/core'
import { ApiFetch} from '../../../../Helpers/ApiCall';
import { PreviewEndpoints } from '../../../../Helpers/AppRoutes';
import { setUserCompanies } from '../../../../modules/actions/SetUserCompanies'
import { withRouter } from 'react-router-dom';
import { useDispatch, useSelector } from "react-redux";

const UserProjectsList = (props) => {
    const userCompanies = useSelector(state => state.UserCompaniesReducer);
    const user = useSelector(state => state.oidc.user);
    const dispatch = useDispatch();

    const CONDITION = {
        0: 'Draft',
        1: 'Lunched',
        2: 'Finished',
        3: 'Banned'
    }

    useEffect(() => {
        const loadList = async () => {
            let userId = user.profile.sub;
            let res = await ApiFetch(PreviewEndpoints.GetUserPreviews + userId);
            if (res !== undefined) {
                dispatch(setUserCompanies(res));
            }

            console.log(123)
        }

        loadList();
    }, [])

    const ListItemClickHandler = (Id) => {
        let path = `/CreateCompany/${Id}`; 
        props.history.push(path);
    }

    const GetStyle = (status) => {
        let style = 'list-item link link-bigger d-flex justify-content-between ';
        switch (status){
            case 0: style += 'muted-link'; break;
            case 3: style += 'banned-link'; break;
            default:
        }

        return style;
    }

    const GetProgress = (item) => {
        let condition;

        switch (item.status){
            case 0: 
                condition = CONDITION[0]; 
                break;
            case 2: 
                condition =  item.progress; 
                break;
            case 3: 
                condition =  CONDITION[3]; 
                break;
            default: 
                condition = item.progress;
        }

        return (
            <span>{condition}</span>
        );
    }

    const GetTitle = (item) => {
        let title = item.title === null ? '---/---/---' : item.title;

        return (
            <span>{title}</span>
        )
    }

    return (
        <List className="list">
            {userCompanies.companies.map((item, idx) => 
                <ListItem 
                    button
                    key={idx} 
                    className={GetStyle(item.status)}
                    onClick={() => ListItemClickHandler(item.id)}>
                    {GetTitle(item)}
                    {GetProgress(item)}
                </ListItem>    
            )}
        </List>
    )
}

export default withRouter(UserProjectsList);