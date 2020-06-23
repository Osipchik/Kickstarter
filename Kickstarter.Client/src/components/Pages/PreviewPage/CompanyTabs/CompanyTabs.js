import React, { useState } from 'react'
import Tabs from "@material-ui/core/Tabs";
import Paper from "@material-ui/core/Paper";
import StoryPage from "../StoryPage/StoryPage";
import { StyledBadge } from "../../../Decorators/StyleBadge";
import { CompanyHeader } from '../PageHeader/CompanyHeader';
import { LinkTab, SwipeTabs } from "../../../Decorators/SwipeTabs/SwipeTabs";
import { StoryTab } from './StoryTab';


export const CompanyTabs = (props) => {
    const [pageIndex, setIndex] = useState(0);

    const getPreviewHeader = () => {
        const {title, description, videoUrl, previewImage} = props.basic;
        const {goal, endDate} = props.founding;
        
        return(
            <CompanyHeader
                title={title ? title : 'Project title'}
                description={description ? description : 'Description of your project'}
                media={videoUrl ? videoUrl : previewImage.url}
                endDate={endDate}
                goal={goal}
            />
        )
    }

    const getHeader = () => {
        return (
            <CompanyHeader
                id={props.id}
            />
        )
    }

    const getPreviewPages = () => {
        const {risks, editorState} = props.editor;

        let pages = [
            <StoryTab risks={risks} story={editorState}/>,
            '111',
            '222',
            '333',
            '444'
        ]

        return pages;
    }

    const getPages = () => {
        let pages = [
            '000',
            '111',
            '222',
            '333',
            '444'
        ]

        return pages;
    }

    return(
        <div >
            <div className="section-lg">
                {props.isPreview ? getPreviewHeader() : getHeader()}
            </div>
            <br/>
            <Paper elevation={3} square className="sticky-top d-flex justify-content-center">
                <Tabs
                    TabIndicatorProps={{ children: <div /> }}
                    className="p-0"
                    indicatorColor="primary"
                    variant="scrollable"
                    value={pageIndex}
                    onChange={(e, index) => setIndex(index)}
                    scrollButtons="auto"
                    aria-label="nav tabs example">
                    <LinkTab href="/Preview/" index={0} 
                             label={"Campaign"}
                    />
                    <LinkTab href="/Preview/1" index={1}
                             label={
                                 <StyledBadge badgeContent={4} className="green-main mt-3">
                                     <p className="text-dark mr-2">FAQ</p>
                                 </StyledBadge>
                             }
                    />
                    <LinkTab href="/Preview/2" index={2}
                             label={
                                 <StyledBadge badgeContent={12} className="green-main mt-3">
                                     <p className="text-dark mr-3">Updates</p>
                                 </StyledBadge>}
                    />
                    <LinkTab href="/Preview/3" index={3}
                             label={
                                 <StyledBadge badgeContent={3025} max={9999} className="green-main mt-3">
                                     <p className="text-dark mr-3">Comments</p>
                                 </StyledBadge>}
                    />
                    <LinkTab label="Community" href="/Preview/4" index={4} />
                </Tabs>
            </Paper>
            <div className="fill-section">
                <section className="bg-white">
                    <SwipeTabs
                        selectedPage={pageIndex}
                        onChangePage={(index) => setIndex(index)}
                        pages={props.isPreview ? getPreviewPages() : getPages()}
                    />
                </section>
            </div>
        </div>
    )
}