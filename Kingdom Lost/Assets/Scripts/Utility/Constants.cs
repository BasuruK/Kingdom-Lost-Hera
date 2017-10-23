using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
	public static int current_level = 1;
    // backdrop tiliting speed
    public static float offset_speed = 0.0008f;
    // gap between player and enemy
    public static float p_e_gap = 12f;
    // activate run gap
    public static float p_e_run_gap = 18f;
    // walk speed
    public static float level_1_enemy_walk = 1.0f;
    // run speed
    public static float level_1_enemy_run = 5.0f;
    // attack mode 1 distance
    public static float level_1_enemy_attack_mode_1_distance = 14f;

	// attack mode 1 distance
	public static float level_2_Walker_attack_mode_1_distance = 8f;

    // view margins
    public static float view_margin_left = 10f;
    public static float view_margin_right = 50f;
    public static float enemy_margin_right = 20f;

    // game scene 1 (level 1) progress level distances
    public static float level_1_p_1 = 100;
    public static float level_1_p_2 = 300;
    public static float level_1_p_3 = 600;
	public static float level_1_p_4 = 960; // 960
	public static float level_1_max = 1000; // 1000

    // game scene 2 (level 2) progress level distances
    public static float level_2_p_1 = 150;
    public static float level_2_p_2 = 400;
    public static float level_2_p_3 = 800;
	public static float level_2_p_4 = 1400; // 1400
	public static float level_2_max = 2000; // 2000 (ground tile size = 697.54)

    // game objects TAGs
    public static string main_character = "KingDutugamunu";
    public static string main_character_weapon = "sword";

    public static string main_character_l2 = "MainCharacter";
	public static string main_character_weapon_l2 = "sword_l2";

    // health damage with distance
    public static float damage_distance_a = 12f;
    public static float damage_distance_b = 6f;

    // health damage points with distance
    public static float damage_distance_a_points = 8f;
    public static float damage_distance_b_points = 12f;
}
