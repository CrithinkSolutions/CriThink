import React, { Component } from "react";
import { Redirect } from 'react-router-dom'
import { WaveUpDown, WaveDown } from "./../Layout";
import { Menu, Segment, Label, Popup, Grid, Loader } from "semantic-ui-react";
import { ReactComponent as Logo } from './../../svg/logoround.svg';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { getQuestions, getNews } from '../../actions/demo';
import './../../fonts/fonts.css'
import "./../../custom.css";

class MenuRender extends Component {
	render() {
		return (
			<Segment color={this.props.color} textAlign="left" className="noborderafter light">
				<h3>{this.props.header}</h3>
				{this.props.children}
			</Segment>
		);
	}
}


class AnalysisNews extends Component {
	render() {
		return(
			<Grid padded >
				<Grid.Row column={2}>
					<Grid.Column width={15}>
						<Segment basic>{this.props.body}</Segment>
					</Grid.Column>
					<Grid.Column >
						{this.props.children}
					</Grid.Column>
				</Grid.Row>
			</Grid>
		)
	}
}

class LogoAnim extends Component {
	render() {
		return (
			<Popup content={this.props.msgPopup} trigger={
				<Segment basic>
					 <div className="flip-card">
					  <div className="flip-card-inner">
					    <div className="flip-card-front">
					      <Logo />
					    </div>
					    <div className="flip-card-back">
					      <span>{this.props.letter}</span>
					    </div>
					  </div>
					</div>
				</Segment>
			} />
		)
	}
}

class AnalysisArea extends Component {

	componentDidMount() {
		this.props.getQuestions()
		this.props.getNews()
	}

	state = { activeItem: "OVERVIEW" };

	handleItemClick = (e, { name }) => this.setState({ activeItem: name });

	switchRender() {
		switch (this.state.activeItem) {
			case "OVERVIEW":
				return (
					<MenuRender color="red" header="DOMAIN IS NOT TRUSTED">
						<Segment basic>
							Lorem ipsum dolor sit amet, consectetur adipiscing
							elit, sed do eiusmod tempor incididunt ut labore et
							dolore magna aliqua. Ut enim ad minim veniam, quis
							nostrud exercitation ullamco laboris nisi ut aliquip
							ex ea commodo consequat. Duis aute irure dolor in
							reprehenderit in voluptate velit esse cillum dolore
							eu fugiat nulla pariatur. Excepteur sint occaecat
							cupidatat non proident, sunt in culpa qui officia
							deserunt mollit anim id est laborum. Sed ut
							perspiciatis unde omnis iste natus error sit
							voluptatem accusantium doloremque laudantium, totam
							rem aperiam, eaque ipsa quae ab illo inventore
							veritatis et quasi architecto.
						</Segment>
					</MenuRender>
				);
			case "KEY WORDS":
				return (
					<MenuRender color="grey" header="Section under construction...">
					</MenuRender>
				);
			case "ANALYSIS":
				return (
					<MenuRender header={this.props.newsHeader}>
						{this.props.newsBody ? 
							<AnalysisNews body={this.props.newsBody}>
								<LogoAnim msgPopup={this.props.head} letter='H'/>
								<LogoAnim msgPopup={this.props.evidence} letter='E' />
								<LogoAnim msgPopup={this.props.accurancy} letter='A' />
								<LogoAnim msgPopup={this.props.deceiving} letter='D' />
							</AnalysisNews> : <Loader active />
						}
					</MenuRender>
				);
			case "CHECK ANOTHER NEWS":
				return (
					<Redirect to='/3'/>
				);
			default:
				return null;
		}
	}

	render() {
		const { activeItem } = this.state;
		return (
			<div>
				<WaveDown namePage="Check Your News">
					<Menu pointing secondary size="huge" >
						<Menu.Item
							name="OVERVIEW"
							active={activeItem === "OVERVIEW"}
							onClick={this.handleItemClick}
						/>
						<Menu.Item
							name="KEY WORDS"
							active={activeItem === "KEY WORDS"}
							onClick={this.handleItemClick}
						/>
						<Menu.Item
							name="ANALYSIS"
							active={activeItem === "ANALYSIS"}
							onClick={this.handleItemClick}
						/>
						<Menu.Menu position='right'>
			            <Menu.Item
			              name='CHECK ANOTHER NEWS'
			              active={activeItem === 'CHECK ANOTHER NEWS'}
			              onClick={this.handleItemClick}
			            />
			          </Menu.Menu>
					</Menu>
					{this.switchRender()}
				</WaveDown>
			</div>
		);
	}
}

function mapStateToProps(state) {
  return {
    head: state.demo.questionH,
    evidence: state.demo.questionE,
    accurancy: state.demo.questionA,
    deceiving: state.demo.questionD,
    newsHeader: state.demo.newsHeader,
    newsBody: state.demo.newsBody
  };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({
        getQuestions,
        getNews
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(AnalysisArea);