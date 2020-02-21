from typing import List
import re
import pandas as pd


def get_properties_df(filtered, cols, vec_str):
	properties_df: pd.DataFrame = filtered[[col for col in cols if vec_str in col]]
	properties_df.columns = [col.split(vec_str)[1] for col in properties_df.columns]
	return properties_df


class PyGameVector:
	def __init__(self, game_state: pd.DataFrame):
		cols = [col for col in game_state.columns if "Player" not in col]
		filtered = game_state[cols]

		self.auras: pd.DataFrame = filtered[[col for col in cols if "Aura" in col]]
		self.player1 = PyControllerVector(game_state, 1)
		self.player2 = PyControllerVector(game_state, 2)
		self.one_turn_effect_enchantments: pd.DataFrame = filtered[[col for col in cols if "Effect" in col]]

		properties = set(cols) - set(self.auras.columns) - set(self.one_turn_effect_enchantments.columns)
		self.properties: pd.DataFrame = filtered[properties]


class PyHearthVector:
	def __init__(self, filtered: pd.DataFrame, cols: List[str], vec_str: str):
		self.properties: pd.DataFrame = get_properties_df(filtered, cols, vec_str)


class PyControllerVector:
	def __init__(self, game_state: pd.DataFrame, player_idx: int):
		# super().__init__(game_state)
		self.player_idx = player_idx
		cols = [col for col in game_state.columns if f"Player{str(self.player_idx)}" in col]
		filtered = game_state[cols]

		self.properties: pd.DataFrame = filtered[[col for col in cols if len(col.split(".")) == 3 or "LastCard" in col]]
		self.properties.columns = [col.split(f"Player{str(self.player_idx)}.")[1] for col in self.properties.columns]

		self.controller_aura_effects: pd.DataFrame = filtered[[col for col in cols if "ControllerAuraEffects" in col]]
		self.controller_aura_effects.columns = [col.split("ControllerAuraEffects.")[1] for col in self.controller_aura_effects.columns]

		self.board: PyZoneVector = PyZoneVector(filtered, cols, "BoardZone.")
		self.deck: PyDeckVector = PyDeckVector(filtered, cols)
		self.hand: PyZoneVector = PyZoneVector(filtered, cols, "HandZone.")
		self.hero: PyHeroVector = PyHeroVector(filtered, cols)
		self.graveyard: PyZoneVector = PyZoneVector(filtered, cols, "GraveyardZone.")
		self.secret_zone: PyZoneVector = PyZoneVector(filtered, cols, "SecretZone.")

	def __repr__(self):
		return f"Player{self.player_idx}"

	def __str__(self):
		return f"Player{self.player_idx}"


class PyZoneVector(PyHearthVector):
	def __init__(self, filtered: pd.DataFrame, cols: List[str], vec_str: str):
		super().__init__(filtered, cols, vec_str)

		card_cols = [col for col in self.properties.columns if re.match("Card\\d", col)]
		self.cards = self.properties[card_cols]
		self.cards.columns = [[col.split(".")[1] for col in card_cols]]


class PyDeckVector(PyZoneVector):
	def __init__(self, filtered: pd.DataFrame, cols: List[str], vec_str: str = "DeckZone."):
		super().__init__(filtered, cols, vec_str)


class PyHeroVector(PyHearthVector):
	def __init__(self, filtered: pd.DataFrame, cols: List[str], vec_str: str = "Hero."):
		super().__init__(filtered, cols, vec_str)
		self.hero_power = PyHeroPower(filtered, cols, "HeroPower.")


class PyHeroPower(PyHearthVector):
	def __init__(self, filtered: pd.DataFrame, cols: List[str], vec_str: str = "HeroPower."):
		super().__init__(filtered, cols, vec_str)

