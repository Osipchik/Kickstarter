import React, {Component} from 'react';
import Tabs from "@material-ui/core/Tabs";
import Paper from "@material-ui/core/Paper";
import { BasicPage } from "./BasicPage/BasicPage";
import { FoundingPage } from "./FoundingPage/FoundingPage";
import StoryPage from "./StoryPage/StoryPage";
import RewardsPage from "./RewardsPage/RewardsPage";
import {LinkTab, SwipeTabs} from "../../Decorators/SwipeTabs/SwipeTabs";
import '../../../styles/LinkStyle.css';
import '../../../styles/ComponentText.css'

export class CreateCompany extends Component {
    max = 5;
    
    constructor(props) {
        super(props);
        
        this.companyId = props.match.params.companyId;

        const page = props.match.params.page;
        const select = Number(page >= 0 && page <= this.max ? page : 0);
        
        this.state = {
            value: select
        };
    }
    
    handleChange = (event, newValue) => {
        this.setState({
            value: newValue
        })
    };

    handleSwipe = (newValue) => {
        this.setState({
            value: newValue
        })
    };
    
    render() {
        return (
            <div >
                <Paper elevation={3} square className="sticky-top d-flex justify-content-center">
                    <Tabs
                        TabIndicatorProps={{ children: <div /> }}
                        className="p-0"
                        indicatorColor="primary"
                        variant="scrollable"
                        value={this.state.value}
                        onChange={this.handleChange}
                        scrollButtons="auto"
                        aria-label="nav tabs example">
                        <LinkTab label="Basics" href="/CreateCompany/" index={0} />
                        <LinkTab label="Founding" href="/CreateCompany/1" index={1} />
                        <LinkTab label="Rewards" href="/CreateCompany/2" index={2} />
                        <LinkTab label="Story" href="/CreateCompany/3" index={3} />
                        <LinkTab label="People" href="/CreateCompany/4" index={4} />
                        <LinkTab label="Launch" href="/CreateCompany/5" index={5} />
                    </Tabs>
                </Paper>
                <section className="bg-white">
                    <SwipeTabs 
                        selectedPage={this.state.value} 
                        onChangePage={this.handleSwipe}
                        pages={[
                            <BasicPage companyId={this.companyId}/>,
                            <FoundingPage companyId={this.companyId}/>,
                            <RewardsPage companyId={this.companyId}/>,
                            <StoryPage companyId={this.companyId}/>,
                            'sss',
                            'ddd',
                        ]}
                    />
                </section>
            </div>
        );
    }
}
