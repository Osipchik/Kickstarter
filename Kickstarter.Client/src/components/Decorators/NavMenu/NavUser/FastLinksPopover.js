import React from 'react'
import { Link } from 'react-router-dom';
import { AccountCol } from "./AccountCol";
import UserProjectsList from "./CreatedProjectsCol";
import Paper from '@material-ui/core/Paper';
import { Button } from '@material-ui/core';
import ClickAwayListener from '@material-ui/core/ClickAwayListener';
import AddIcon from '@material-ui/icons/Add';
import userManager from '../../../../auth/userManager';
import { withRouter } from 'react-router-dom';
import { CreateCompany } from '../../../../Helpers/ApiCall';
import '../../../../styles/FastLinks.css';


const LinksPopover = (props) => {

    const createCompany = async (history) => {
        let result = await CreateCompany();
        if (result !== null){
            history.push('/CreateCompany/' + result.id)
        }
    }

    const Popover = () => {
        return(
            <ClickAwayListener onClickAway={props.HandleClose}>
                <Paper elevation={5} className="popup">
                    <div className="py2">
                        <div className="row">
                            <div className="pl4 pr2">
                                <div className="text-uppercase pb18">
                                    YOUR ACCOUNT
                                </div>
                                <AccountCol />
                            </div>
                            <div className="col pr4 pl4-md pt2-md">
                                <div className="text-uppercase pb18">
                                    CREATED PROJECTS
                                </div>
                                <UserProjectsList/>
                                <Button
                                    variant="contained"
                                    size="small"
                                    className="add-btn"
                                    startIcon={<AddIcon />}
                                    onClick={() => createCompany(props.history)}
                                    >
                                        Add new
                                </Button>
                            </div>
                        </div>
                        <hr/>
                        <Link to={"/"} className="link link-bigger pl3 pb2" onClick={async (event) => {
                            event.preventDefault();
                            userManager.removeUser();
                            await userManager.signoutRedirect();
                            props.HandleClose();
                        }}>
                            Log out
                        </Link>
                    </div>
                </Paper>
            </ClickAwayListener>
        )
    }

    return(
        <div>
            {props.open ? Popover() : null}
        </div>
    )
}

export const FastLinksPopover = withRouter(LinksPopover)