import React, { Component } from "react";
import { Redirect } from "react-router-dom";
import { WaveDown } from "./../Layout";
import { Menu, Segment, Popup, Grid, Loader } from "semantic-ui-react";
import { ReactComponent as Logo } from "./../../svg/logoround.svg";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { getQuestions, getNews } from "../../actions/demo";

class MenuRender extends Component {
	render() {
		return (
			<Segment
				color={this.props.color}
				textAlign="left"
				className="noborderafter light"
			>
				<h3>{this.props.header}</h3>
				{this.props.children}
			</Segment>
		);
	}
}

class AnalysisNews extends Component {
	render() {
		return (
			<Grid padded>
				<Grid.Row column={2}>
					<Grid.Column width={15}>
						<Segment basic>{this.props.body}</Segment>
					</Grid.Column>
					<Grid.Column>{this.props.children}</Grid.Column>
				</Grid.Row>
			</Grid>
		);
	}
}

class LogoAnim extends Component {
	render() {
		return (
			<Popup
				content={this.props.msgPopup}
				trigger={
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
				}
			/>
		);
	}
}

class AnalysisArea extends Component {
	componentDidMount() {
		this.props.getQuestions();
		this.props.getNews(this.props.newsLink);
		switch (this.props.newsClass) {
			case "Trusted":
			case "Satiric":
				this.setState({ color: "green" });
				break;
			case "Fake":
				this.setState({ color: "red" });
				break;
			case "Cospiracy":
				this.setState({ color: "orange" });
				break;
			default:
				return null;
		}
	}

	state = {
		activeItem: "OVERVIEW",
		color: "",
	};

	handleItemClick = (e, { name }) => this.setState({ activeItem: name });

	switchRender() {
		switch (this.state.activeItem) {
			case "OVERVIEW":
				return (
					<MenuRender
						color={this.state.color}
						header={this.props.newsClass}
					>
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
					<MenuRender
						color="grey"
						header="Section under construction..."
					></MenuRender>
				);
			case "ANALYSIS":
				return (
					<MenuRender header={this.props.newsHeader}>
						<AnalysisNews body={this.props.newsBody}>
							<LogoAnim msgPopup={this.props.head} letter="H" />
							<LogoAnim
								msgPopup={this.props.evidence}
								letter="E"
							/>
							<LogoAnim
								msgPopup={this.props.accurancy}
								letter="A"
							/>
							<LogoAnim
								msgPopup={this.props.deceiving}
								letter="D"
							/>
						</AnalysisNews>
					</MenuRender>
				);
			case "CHECK ANOTHER NEWS":
				return <Redirect to="/3" />;
			default:
				return null;
		}
	}

	render() {
		const { activeItem } = this.state;
		return (
			<div>
				{this.props.newsLink == null ? <Redirect to="/3" /> : null}
				<WaveDown namePage="Check Your News">
					<Menu pointing secondary size="huge">
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
						<Menu.Menu position="right">
							<Menu.Item
								name="CHECK ANOTHER NEWS"
								active={activeItem === "CHECK ANOTHER NEWS"}
								onClick={this.handleItemClick}
							/>
						</Menu.Menu>
					</Menu>
					{this.props.newsBody ? (
						this.switchRender()
					) : (
						<Loader active />
					)}
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
		newsBody: state.demo.newsBody,
		newsLink: state.demo.demoNewsSelected.uri,
		newsClass: state.demo.demoNewsSelected.type,
	};
}

function mapDispatchToProps(dispatch) {
	return bindActionCreators(
		{
			getQuestions,
			getNews,
		},
		dispatch
	);
}

export default connect(mapStateToProps, mapDispatchToProps)(AnalysisArea);
