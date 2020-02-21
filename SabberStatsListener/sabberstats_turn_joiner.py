import pandas as pd
import os
from pathlib import Path

from py_hearth_vector import PyControllerVector, PyGameVector


def get_proj_root(cwd: Path):
	for root, dirs, files in os.walk(str(cwd)):
		if "README.md" in files:
			return cwd
		break
	cwd = cwd.parent
	return get_proj_root(cwd)


CWD = get_proj_root(Path().cwd())
DT = "12_02_2019 18_30"
STATS_DIR = CWD.joinpath("HearthVectors", "PALADIN_vs_MAGE", "Tyche", DT)


def get_turn_dirs(root):
	for root, dirs, files in os.walk(root):
		for f in files:
			if "Turn_" in f:
				return [STATS_DIR.joinpath(turn_) for turn_ in dirs]


def get_actions(turn_dir):
	for root, dirs, files in os.walk(turn_dir):
		return files


def transpose_and_reheader(df):
	df = df.T
	header = df.iloc[0]
	df = df[1:]
	df.columns = header
	return df


def parse_state(game_state: pd.DataFrame):
	return PyGameVector(game_state)


def parse_players(game_state: pd.DataFrame):
	players = [PyControllerVector(game_state, 1), PyControllerVector(game_state, 2)]
	return players[0], players[1]


turn_idx = 1
action_idx = 1
turn_idx += 1
game: PyGameVector = parse_state(transpose_and_reheader(pd.read_csv(str(STATS_DIR.joinpath("Turn1", "BasicMage_vs_BasicMage_Turn_1_Action_1.csv")), index_col=False, header=None)))
# game = transpose_and_reheader(game)
# game = parse_state(game)
for turn in get_turn_dirs(STATS_DIR):
	for file in get_actions(turn):
		action = pd.read_csv(STATS_DIR.joinpath(turn, file), index_col=False, header=None)
		action = transpose_and_reheader(action)
		game_idx = 0
		for i in range(len(game.columns)):
			if list(game.columns)[i] != list(action.columns)[i]:
				if list(action.columns)[i] in list(game.columns):
					print(list(action.columns)[i])
				else:
					game.insert(i, list(action.columns)[i], 0)
			game_idx = i
			# if i in range(len(game.columns)):
			# 	if col not in list(game.columns):
			# 		game.insert(i, col, 0, allow_duplicates=True)
		for i in range(len(action.columns))[game_idx:]:
			if list(action.columns)[i] in list(game.columns):
				print(list(action.columns)[i])
			else:
				game[action.columns[i]] = 0
		game = game.append(action)
		turn_idx += 1
game = transpose_and_reheader(game)
